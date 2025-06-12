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




module Main =
    type State = { 
        page: App.Page
        theme: App.Theme
        solver: Solver.State
        puzzle: Puzzle.State
    }

    type Message = 
        | NavigateTo of App.Page
        | SetPuzzle of int list
        | ToggleTheme
        | SolverMessage of Solver.Message
        | PuzzleMessage of Puzzle.Message

    let init () =
        { page = App.MenuPage; solver = Solver.init(); puzzle = Puzzle.init(); theme = App.darkTheme }, Cmd.none

    let update (msg: Message) (state: State) =
        match msg with
        | NavigateTo newPage -> { state with page = newPage }, Cmd.none
        | SetPuzzle puzzle -> { state with puzzle.sudoku = Sudoku.init_puzzle(puzzle) }, Cmd.none
        | ToggleTheme -> { state with theme = (if state.theme.isLight then App.darkTheme else App.lightTheme) }, Cmd.none
        | SolverMessage sm -> { state with solver = (Solver.update sm state.solver) }, Cmd.none
        | PuzzleMessage pm -> { state with puzzle = (Puzzle.update pm state.puzzle) }, Cmd.none


    let view (state: State) (dispatch): IView =

        let navigate = (fun page -> dispatch (NavigateTo page))

        let setPuzzle = (fun puzzle -> (
            dispatch (SetPuzzle puzzle)
            dispatch (NavigateTo App.PuzzlePage)
        ))

        let toggleTheme = (fun _ -> (
            let app = Application.Current
            app.RequestedThemeVariant <- (
                if state.theme.isLight then
                    Styling.ThemeVariant.Dark
                else
                    Styling.ThemeVariant.Light
            )
            dispatch ToggleTheme
        ))

        match state.page with
        | App.MenuPage -> Menu.view navigate toggleTheme
        | App.SolverPage -> Solver.view state.solver state.theme (fun msg -> dispatch (SolverMessage msg)) navigate
        | App.PuzzleMenuPage -> PuzzleMenu.view setPuzzle navigate
        | App.PuzzlePage -> Puzzle.view state.puzzle state.theme (fun msg -> dispatch (PuzzleMessage msg)) navigate





type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Sudoku"
        //base.Height <- 400.0
        //base.Width <- 400.0

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        Elmish.Program.mkProgram Main.init Main.update Main.view
        //Elmish.Program.mkProgram Solver.init Solver.update Solver.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

type SudokuApp() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<SudokuApp>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)