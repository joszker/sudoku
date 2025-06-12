module PuzzleMenu
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


let puzzle1 = [
    3;0;2; 0;0;0; 0;8;9;
    0;6;8; 0;5;2; 7;3;4;
    0;0;9; 0;0;0; 0;0;0;

    4;0;0; 0;0;7; 0;0;0;
    0;8;3; 2;0;1; 5;9;0;
    0;0;0; 5;0;0; 0;0;2;

    0;0;0; 0;0;0; 2;0;0;
    2;1;4; 7;8;0; 3;5;0;
    5;3;0; 0;0;0; 9;0;8
]


let puzzle2 = [
    0;0;0; 0;6;4; 8;1;0;
    0;4;0; 0;5;0; 0;6;2;
    0;0;9; 0;1;0; 3;0;0;

    0;0;3; 0;4;0; 6;0;7;
    0;0;8; 1;0;7; 5;0;0;
    7;0;4; 0;3;0; 1;0;0;

    0;0;6; 0;7;0; 2;0;0;
    4;3;0; 0;8;0; 0;9;0;
    0;1;7; 3;9;0; 0;0;0
]

let puzzle3 = [
    0;0;0; 0;0;0; 0;0;0;
    0;3;0; 0;0;9; 0;5;0;
    0;0;9; 8;2;0; 0;1;3;

    1;0;7; 0;9;0; 0;0;0;
    3;0;0; 0;0;0; 0;4;5;
    0;0;0; 0;0;0; 0;0;6;

    0;2;0; 0;7;4; 0;0;0;
    0;0;0; 9;0;0; 0;0;0;
    0;6;0; 1;0;5; 4;0;0
]

let view (setPuzzle) (navigate): IView =
    Grid.create [
        Grid.rowDefinitions "*,Auto,*"
        Grid.children [

            // Title
            TextBlock.create [
                Grid.row                        0
                TextBlock.fontSize              36.0
                TextBlock.verticalAlignment     VerticalAlignment.Center
                TextBlock.horizontalAlignment   HorizontalAlignment.Center
                TextBlock.text                  "Select a Puzzle"
            ]

            // Buttons
            StackPanel.create [
                Grid.row                        1
                StackPanel.orientation          Orientation.Vertical
                StackPanel.spacing              30
                StackPanel.width                160
                StackPanel.horizontalAlignment  HorizontalAlignment.Center
                StackPanel.verticalAlignment    VerticalAlignment.Center
                StackPanel.children [
                    Button.create [
                        Button.onClick (fun _ -> setPuzzle puzzle1)
                        Button.content "Easy"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    Button.create [
                        Button.onClick (fun _ -> setPuzzle puzzle2)
                        Button.content "Medium"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    Button.create [
                        Button.onClick (fun _ -> setPuzzle puzzle3)
                        Button.content "Hard"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                ]
            ]

            Button.create [
                Button.onClick (fun _ -> navigate App.MenuPage)
                Button.content "← Go Back"
                Button.horizontalAlignment HorizontalAlignment.Left
                Button.verticalAlignment VerticalAlignment.Top
            ]
        ]
    ]