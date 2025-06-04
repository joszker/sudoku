module Menu
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

    
let view (navigate): IView =
    Grid.create [
        Grid.rowDefinitions "*,Auto,*"
        Grid.children [

            // Title
            TextBlock.create [
                Grid.row                        0
                TextBlock.fontSize              48.0
                TextBlock.verticalAlignment     VerticalAlignment.Center
                TextBlock.horizontalAlignment   HorizontalAlignment.Center
                TextBlock.text                  "Sudoku"
            ]

            // Buttons
            StackPanel.create [
                Grid.row                        1
                StackPanel.orientation          Orientation.Vertical
                StackPanel.spacing              10
                StackPanel.width                160
                StackPanel.horizontalAlignment  HorizontalAlignment.Center
                StackPanel.verticalAlignment    VerticalAlignment.Center
                StackPanel.children [
                    Button.create [
                        Button.onClick (fun _ -> navigate Pages.SolverPage)
                        Button.content "Puzzles"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    Button.create [
                        Button.onClick (fun _ -> navigate Pages.SolverPage)
                        Button.content "Solver"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    Button.create [
                        Button.onClick (fun _ -> navigate Pages.SolverPage)
                        Button.content "Exit"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    (*
                    TextBlock.create [
                        TextBlock.fontSize 48.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string state.Current)
                    ]
                    *)
                ]
            ]
        ]
    ]