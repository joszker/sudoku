module Solver
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



let replaceAt (array: 'a list) index newValue  =
    array
    |> List.mapi (fun i v -> if i = index then newValue else v)

let rec solveSudoku (board: Sudoku.Board) (index: int): Sudoku.Board seq =
    if index >= board.Length then
        seq { yield board }
    else if board[index].digit.IsNone then
        [1..9] |> Seq.map (fun digit ->
            replaceAt board index { digit = Some digit; isEditable = true; }
        )
        |> Seq.filter Sudoku.isSolvable
        |> Seq.map (fun board -> (solveSudoku board (index + 1)))
        |> Seq.concat
    else
        solveSudoku board (index + 1)

let solve (board: Sudoku.Board): Sudoku.Board =
    let newBoard = board |> List.map (fun cell -> { cell with isEditable = cell.digit.IsNone })
    Seq.head (solveSudoku newBoard 0)

type State = {
    sudoku: Sudoku.State
}

type Message = 
    | SudokuMessage of Sudoku.Message
    | Solve
    | Reset

let init () = { sudoku = fst (Sudoku.init()) }, Cmd.none

let update (msg: Message) (state: State) =
    match msg with
    | SudokuMessage sm -> { state with sudoku = fst (Sudoku.update sm state.sudoku) }, Cmd.none
    | Solve -> { state with sudoku.board = (solve state.sudoku.board) }, Cmd.none
    | Reset -> { state with sudoku.board = Sudoku.createEmptyBoard() }, Cmd.none

let view (state: State) (dispatch: Message -> unit) (navigate): IView =

    Grid.create [
        Grid.children [
            StackPanel.create [
                StackPanel.orientation          Orientation.Vertical
                StackPanel.spacing              50
                StackPanel.horizontalAlignment  HorizontalAlignment.Center
                StackPanel.verticalAlignment    VerticalAlignment.Center
                StackPanel.width                (40.0 * 9.0)
                StackPanel.children [
                    let isSolved = Sudoku.isSolved state.sudoku.board

                    TextBlock.create [
                        TextBlock.fontSize 18.0
                        TextBlock.text (
                            if isSolved then "Solved" else "Unsolved"
                        )
                        TextBlock.foreground (
                            if isSolved then Colors.Lime else Colors.White
                        )
                        TextBlock.textAlignment TextAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.verticalAlignment VerticalAlignment.Center
                    ]

                    Sudoku.view state.sudoku (fun msg -> dispatch (SudokuMessage msg))

                    StackPanel.create [
                        StackPanel.orientation          Orientation.Horizontal
                        StackPanel.spacing              50
                        StackPanel.horizontalAlignment  HorizontalAlignment.Center
                        StackPanel.verticalAlignment    VerticalAlignment.Center
                        StackPanel.children [
                            Button.create [
                                Button.onClick (fun _ -> dispatch Solve)
                                Button.isEnabled ((not isSolved) && (Sudoku.isSolvable state.sudoku.board))
                                Button.content "Solve"
                                Button.horizontalAlignment HorizontalAlignment.Stretch
                                Button.horizontalContentAlignment HorizontalAlignment.Center
                            ]
                            Button.create [
                                Button.onClick (fun _ -> dispatch Reset)
                                Button.content "Reset"
                                Button.horizontalAlignment HorizontalAlignment.Stretch
                                Button.horizontalContentAlignment HorizontalAlignment.Center
                            ]
                        ]
                    ]
                ]
            ]
            Button.create [
                Button.onClick (fun _ -> navigate Pages.MenuPage)
                Button.content "← Go Back"
                Button.horizontalAlignment HorizontalAlignment.Left
                Button.verticalAlignment VerticalAlignment.Top
            ]
        ]
    ]

