module Puzzle
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



type State = {
    sudoku: Sudoku.State
    solvedPopup: bool
}

type Message = 
    | SudokuMessage of Sudoku.Message
    | Solved
    | OnPopupClosed

let init () = { sudoku = Sudoku.init(); solvedPopup = false }


let update (msg: Message) (state: State) =
    match msg with
    | SudokuMessage sm -> { state with sudoku = (Sudoku.update sm state.sudoku) }
    | Solved -> { state with solvedPopup = true }
    | OnPopupClosed -> { state with solvedPopup = false }

let view (state: State) (theme: App.Theme) (dispatch: Message -> unit) (navigate): IView =

    if Sudoku.isSolved(state.sudoku.board) then
        dispatch Solved

    Grid.create [
        Grid.children [
            StackPanel.create [
                StackPanel.orientation          Orientation.Vertical
                StackPanel.spacing              50
                StackPanel.horizontalAlignment  HorizontalAlignment.Center
                StackPanel.verticalAlignment    VerticalAlignment.Center
                StackPanel.width                (40.0 * 9.0)
                StackPanel.children [
                    Sudoku.view state.sudoku theme (fun msg -> dispatch (SudokuMessage msg))
                ]
            ]
            Button.create [
                Button.onClick (fun _ -> navigate App.MenuPage)
                Button.content "← Go Back"
                Button.horizontalAlignment HorizontalAlignment.Left
                Button.verticalAlignment VerticalAlignment.Top
            ]

            Popup.create [
                Popup.isOpen state.solvedPopup
                Popup.placement PlacementMode.Center
                Popup.topmost true
                Popup.child (
                    Border.create [
                        Border.background Colors.Green
                        Border.padding 10.0
                        Border.child (TextBlock.create [ 
                            TextBlock.text "Congratulations! You have solved the puzzle."
                        ])
                    ]
                )
                Popup.onClosed (fun _ -> ( 
                    dispatch (OnPopupClosed)
                    navigate App.MenuPage
                ))
            ]
        ]
    ]


