module Sudoku
open Avalonia
open Avalonia.Controls
open Avalonia.Themes.Fluent
open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Controls.Shapes
open Avalonia.Controls.Primitives
open Avalonia.Input
open Avalonia.Interactivity

type Cell = {
    digit: int option
    isEditable: bool
}
type Board = Cell list

type State = {
    board: Board
    hovered: int option
    selected: int option
}

let createEmptyBoard () : Board =
    List.init (9*9) (fun _ -> { digit = None; isEditable = true })

let init () = { board = createEmptyBoard(); hovered = None; selected = None; }, Cmd.none

type Message = 
    | UpdateCell of index: int * digit: int option
    | SetHovered of index: int option
    | SetSelection of index: int option

let update (msg: Message) (state: State) =
    match msg with
    | UpdateCell (targetIndex, newDigit) ->
        let newBoard =
            state.board
            |> List.mapi (fun index cell ->
                if index = targetIndex && cell.isEditable then
                    { cell with digit = newDigit }
                else cell
            )
        { state with board = newBoard }, Cmd.none
    | SetHovered (index) -> { state with hovered = index }, Cmd.none
    | SetSelection (index) -> { state with selected = index }, Cmd.none

let view (state: State) (dispatch): IView =

    let cellSize = 40.0
    let bgColor = Color.Parse "#101010"
    let bgHoverColor = Color.Parse "#202020"
    let bgSelectedColor = Color.Parse "#522F22"
    let bgHighlightedColor = Color.Parse "#443028"//Color.Parse "#303030"
    let bgUneditableColor = Color.Parse "#263238"
    let innerBorderColor = Color.Parse "#804532"
    let outerBorderColor = Color.Parse "#FF8A65"
    let selectedBorderColor = Color.Parse "#FF8A65"
    let digitColor = Colors.White
    let uneditableDigitColor = Colors.Yellow


    Canvas.create [
        Canvas.width (cellSize * 9.0)
        Canvas.height (cellSize * 9.0)

        let rects: IView list = state.board |> List.mapi (fun index cell ->
            let x = index % 9
            let y = index / 9

            Rectangle.create [
                Rectangle.left (float x * cellSize)
                Rectangle.top (float y * cellSize)
                Rectangle.width cellSize
                Rectangle.height cellSize

                Rectangle.fill (
                    if state.selected = Some index then
                        SolidColorBrush(bgSelectedColor)
                    else if state.hovered = Some index then
                        SolidColorBrush(bgHoverColor)
                    else if state.selected.IsSome && cell.digit.IsSome && state.board[state.selected.Value].digit = cell.digit then
                        SolidColorBrush(bgHighlightedColor)
                    else if not cell.isEditable then
                        SolidColorBrush(bgUneditableColor)
                    else
                        SolidColorBrush(bgColor)
                )

                Rectangle.horizontalAlignment HorizontalAlignment.Stretch
                Rectangle.verticalAlignment VerticalAlignment.Stretch

                Rectangle.onPointerEntered (fun _ -> dispatch (SetHovered (Some index)))
                Rectangle.onPointerExited (fun _ -> dispatch (SetHovered None))

                Rectangle.onPointerPressed (fun args ->
                    // Focus the rectangle when clicked so it can receive keyboard input
                    (args.Source :?> InputElement).Focus() |> ignore

                    dispatch (SetSelection (Some index))
                )

                Rectangle.focusable true
                Rectangle.onKeyDown (fun args ->
                    match args.Key with
                    | Key.D1 -> dispatch (UpdateCell(index, Some 1))
                    | Key.D2 -> dispatch (UpdateCell(index, Some 2))
                    | Key.D3 -> dispatch (UpdateCell(index, Some 3))
                    | Key.D4 -> dispatch (UpdateCell(index, Some 4))
                    | Key.D5 -> dispatch (UpdateCell(index, Some 5))
                    | Key.D6 -> dispatch (UpdateCell(index, Some 6))
                    | Key.D7 -> dispatch (UpdateCell(index, Some 7))
                    | Key.D8 -> dispatch (UpdateCell(index, Some 8))
                    | Key.D9 -> dispatch (UpdateCell(index, Some 9))
                    | Key.Back -> dispatch (UpdateCell(index, None))
                    | _ -> ()
                )

            ]
        )
        
        let digits: IView list = state.board |> List.mapi (fun index cell ->
            let text =
                match cell.digit with
                | Some v -> string v
                | None -> ""

            let x = index % 9
            let y = index / 9

            TextBlock.create [
                TextBlock.left (float x * cellSize)
                TextBlock.top ((float y * cellSize) + 3.0)
                TextBlock.width cellSize
                TextBlock.height cellSize
                TextBlock.fontSize 24.0
                TextBlock.text text
                TextBlock.textAlignment TextAlignment.Center
                TextBlock.horizontalAlignment HorizontalAlignment.Center
                TextBlock.verticalAlignment VerticalAlignment.Center
                TextBlock.padding 0.0
                TextBlock.margin 0.0
                TextBlock.isHitTestVisible false
                TextBlock.foreground (
                    if cell.isEditable then
                        Colors.White
                    else
                        Colors.Yellow
                )
            ]
        )

        let vlines: IView list = [0..8] |> List.map (fun x ->
            let p = (float x) * cellSize + 0.5
            Line.create [
                Line.startPoint (Point(p, 0.0))
                Line.endPoint (Point(p, cellSize * 9.0))
                Line.stroke  (SolidColorBrush(innerBorderColor))
                Line.strokeThickness 1.0
            ]
        )

        let hlines: IView list = [0..8] |> List.map (fun y ->
            let p = (float y) * cellSize + 0.5
            Line.create [
                Line.startPoint (Point(0.0, p))
                Line.endPoint (Point(cellSize * 9.0, p))
                Line.stroke  (SolidColorBrush(innerBorderColor))
                Line.strokeThickness 1.0
            ]
        )

        let vlinesOuter: IView list = [0..3] |> List.map (fun x ->
            let p = (float x) * 3.0 * cellSize + 0.5
            Line.create [
                Line.startPoint (Point(p, 0.0))
                Line.endPoint (Point(p, cellSize * 9.0))
                Line.stroke  (SolidColorBrush(outerBorderColor))
                Line.strokeThickness 3.0
            ]
        )

        let hlinesOuter: IView list = [0..3] |> List.map (fun y ->
            let p = (float y) * 3.0 * cellSize + 0.5
            Line.create [
                Line.startPoint (Point(0.0, p))
                Line.endPoint (Point(cellSize * 9.0, p))
                Line.stroke  (SolidColorBrush(outerBorderColor))
                Line.strokeThickness 3.0
            ]
        )

        let selectionRect: IView list = [
            if state.selected.IsSome then
                Rectangle.create [
                    let x = state.selected.Value % 9
                    let y = state.selected.Value / 9
                    Rectangle.left  (float x * cellSize)
                    Rectangle.top (float y * cellSize)
                    Rectangle.width cellSize
                    Rectangle.height cellSize
                    Rectangle.fill Colors.Transparent
                    Rectangle.stroke outerBorderColor
                    Rectangle.strokeThickness 1.0
                    Rectangle.isHitTestVisible false
                ]
        ]

            
        Canvas.children (rects @ vlines @ hlines @ vlinesOuter @ hlinesOuter @ digits @ selectionRect)
    ]


let rows (board: Board): Cell list list =
    [ board[0..8]; board[9..17]; board[18..26]; board[27..35]; board[36..44]; board[45..53]; board[54..62]; board[63..71]; board[72..80]; ]


let columns (board: Board): Cell list list =
    let rows = rows board
    [0..8] |> List.map (fun index -> rows |> List.map (fun row -> row[index] ))

let boxes (board: Board): Cell list list =
    [
        board[0..2] @ board[9+0..9+2] @ board[18+0..18+2];
        board[3..5] @ board[9+3..9+5] @ board[18+3..18+5];
        board[6..8] @ board[9+6..9+8] @ board[18+6..18+8];

        board[27..29] @ board[9+27..9+29] @ board[18+27..18+29];
        board[30..32] @ board[9+30..9+32] @ board[18+30..18+32];
        board[33..35] @ board[9+33..9+35] @ board[18+33..18+35];

        board[54..56] @ board[9+54..9+56] @ board[18+54..18+56];
        board[57..59] @ board[9+57..9+59] @ board[18+57..18+59];
        board[60..62] @ board[9+60..9+62] @ board[18+60..18+62];
    ]


let allUnique (cells: Cell list): bool =
    let nonEmpty = cells |> List.filter (fun cell -> cell.digit.IsSome)
    let digits = nonEmpty |> List.map (fun cell -> cell.digit.Value)
    digits.Length = (Set.ofList digits).Count

let isSolvable (board: Board): bool =
    rows board
    |> Seq.append (columns board)
    |> Seq.append (boxes board)
    |> Seq.forall (fun cells -> allUnique cells)


let isSolved (board: Board): bool =
    isSolvable(board) && board |> Seq.forall (fun cell -> cell.digit.IsSome)

