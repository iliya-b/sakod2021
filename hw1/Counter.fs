namespace hw1

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia.Media
    open Avalonia.Media.Imaging
    open System.IO
    
    let convertBytes (n: int64) = if n > 1024L*1024L then sprintf "%u MB" (n / 1024L*1024L) else sprintf "%u B" n

    let dialog = new OpenFileDialog()

    let view (window: Window)   = 
        Component(fun ctx ->
        let filePath = ctx.useState "/home/ilya/default_picture.png"
        let bitmap = new Bitmap(filePath.Current)
        let ImageBrush = new ImageBrush(bitmap)
        ImageBrush.Stretch <- Stretch.UniformToFill
        let FileInfo = new FileInfo(filePath.Current)

        let handleClickAsync () =
            let dialog: OpenFileDialog =
                let filter =
                    FileDialogFilter(Name = "Images", Extensions = ResizeArray([ "jpg" ; "jpeg" ; "png"; "bmp" ; "tiff" ; "gif"; "pnga" ]))

                OpenFileDialog(
                    AllowMultiple = false,
                    Filters = ResizeArray([ filter ]),
                    Title = "Select an Image"
                )

            async {
                let! files = dialog.ShowAsync window |> Async.AwaitTask
                filePath.Set files.[0]
            }
            |> Async.Start

        DockPanel.create [
            DockPanel.background ImageBrush
            DockPanel.width ImageBrush.Source.PixelSize.Width
            DockPanel.height ImageBrush.Source.PixelSize.Height
            DockPanel.verticalAlignment VerticalAlignment.Center
            DockPanel.horizontalAlignment HorizontalAlignment.Center

            DockPanel.children [
                Button.create [
                    Button.dock Dock.Top
                    Button.onClick (fun _ -> handleClickAsync())
                    Button.content "select a file"
                ]
                
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 32.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (
                        sprintf "Dimension: %d x %d \nFile size %s" ImageBrush.Source.PixelSize.Width ImageBrush.Source.PixelSize.Height (convertBytes (FileInfo.Length))
                    )
                ]
            ]
        ]
    )

 
