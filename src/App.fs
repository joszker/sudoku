module App
open Avalonia.Media

type Page =
    | MenuPage
    | SolverPage
    | PuzzleMenuPage
    | PuzzlePage


type Theme = {
    isLight: bool
    bg: Color
    bgHover: Color
    bgSelected: Color
    bgHighlighted: Color
    bgUneditable: Color
    innerBorder: Color
    outerBorder: Color
    selectedBorder: Color
    digit: Color
    uneditableDigit: Color
    text: Color
    green: Color
}

let darkTheme: Theme = {
    isLight = false;
    bg = Color.Parse "#101010";
    bgHover = Color.Parse "#303030";//Color.Parse "#202020"
    bgSelected = Color.Parse "#522F22";
    bgHighlighted = Color.Parse "#443028";//Color.Parse "#303030"
    bgUneditable = Color.Parse "#263238";
    innerBorder = Color.Parse "#804532";
    outerBorder = Color.Parse "#FF8A65";
    selectedBorder = Color.Parse "#FF8A65";
    digit = Colors.White;
    uneditableDigit = Colors.Yellow;
    text = Colors.White;
    green = Colors.Lime;
}

let lightTheme: Theme = {
    isLight = true;
    bg = Color.Parse "#F3F3F3";
    bgHover = Color.Parse "#E3E3E3";
    bgSelected = Color.Parse "#ECD5D5";
    bgHighlighted = Color.Parse "#B7B7B7";
    bgUneditable = Color.Parse "#D0D0D0";
    innerBorder = Color.Parse "#303030";
    outerBorder = Colors.Black;
    selectedBorder = Colors.Red;
    digit = Colors.Black;
    uneditableDigit = Colors.Blue;
    text = Colors.Black;
    green = Colors.Green;
}