# Welcome to Marv!

Marv is a File-Watching Markdown Viewer designed to assist markdown authors on the Windows platform.

## Get Started

Using Marv is easy:

* Use the File->Open menu to select a markdown file;
* Marv will convert the markdown into HTML and render it in the window;
* Edit the markdown file with any editor you like;
* When you save the markdown file in your editor, Marv will notice and update it's HTML rendering.

That's all there is to it.

## How it works

Under the hood, Marv uses a DispatchTimer to poll the selected file, watching the file's LastUpdatedTime.

When that timestamp changes, Marv reads the file, parses the markdown using the *MarkdownSharp* library, 
and renders it in the window using a WPF WebBrowser control.
