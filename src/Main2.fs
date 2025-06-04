open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media

module Main =

    let view () =
        Component(fun ctx ->
            let state = ctx.useState 0

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
                                Button.onClick (fun _ -> state.Set(state.Current - 1))
                                Button.content "Puzzles"
                                Button.horizontalAlignment HorizontalAlignment.Stretch
                                Button.horizontalContentAlignment HorizontalAlignment.Center
                            ]
                            Button.create [
                                Button.onClick (fun _ -> state.Set(state.Current + 1))
                                Button.content "Solver"
                                Button.horizontalAlignment HorizontalAlignment.Stretch
                                Button.horizontalContentAlignment HorizontalAlignment.Center
                            ]
                            Button.create [
                                Button.onClick (fun _ -> state.Set(state.Current + 1))
                                Button.content "Settings"
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
        )

type MainWindow() =
    inherit HostWindow()

    let currentPage = AvaloniaProperty.Register<MainWindow, Component>("CurrentPage")

    do
        base.SetValue(currentPage, Main.view())

        base.Title <- "Sudoku"
        base.Content <- base.GetValue(currentPage) //Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)