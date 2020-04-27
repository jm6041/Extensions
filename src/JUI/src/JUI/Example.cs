using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JUI
{
    public class Example
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Expanded { get; set; }
        public IEnumerable<Example> Children { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    public class ExampleData
    {
        public static readonly Example[] AllExamples = new[] {
        new Example()
        {
            Name = "First Look",
            Path = "/",
            Icon = "&#xe88a"
        },
        new Example()
        {
            Name = "General",
            Expanded = true,
            Children = new [] {
                new Example()
                {
                    Name = "Button",
                    Path = "button",
                    Icon = "&#x853"
                },
                new Example()
                {
                    Name = "Gravatar",
                    Path = "gravatar",
                    Icon = "&#xe84e"
                },
                new Example()
                {
                    Name = "SplitButton",
                    Path = "splitbutton",
                    Icon = "&#xe05f"
                },
                new Example()
                {
                    Name = "Icon",
                    Path = "icon",
                    Icon = "&#xe84f"
                },
                new Example()
                {
                    Name = "Image",
                    Path = "image",
                    Icon = "&#xe8aa"
                },
                new Example()
                {
                    Name = "Link",
                    Path = "link",
                    Icon = "&#xe157"
                },
                new Example()
                {
                    Name = "Login",
                    Path = "login",
                    Icon = "&#xe8e8"
                },
                new Example()
                {
                    Name = "ProgressBar",
                    Path = "progressbar",
                    Icon = "&#xe893",
                    Tags = new [] { "progress", "spinner" }
                },
                new Example()
                {
                    Name = "Dialog",
                    Path = "dialog",
                    Icon = "&#xe8a7",
                    Tags = new [] { "popup", "window" }
                },
                new Example()
                {
                    Name = "Notification",
                    Path = "notification",
                    Icon = "&#xe85a",
                    Tags = new [] { "message", "alert" }
                },
                new Example()
                {
                    Name = "Menu",
                    Path = "menu",
                    Icon = "&#xe91a",
                    Tags = new [] { "navigation", "dropdown" }
                },
                new Example()
                {
                    Name = "Upload",
                    Path = "example-upload",
                    Icon = "&#xe2c6",
                    Tags = new [] { "upload", "file"}
                }
            }
        },
        new Example()
        {
            Name="Containers",
            Children = new [] {
                new Example()
                {
                    Name = "Accordion",
                    Path = "accordion",
                    Icon = "&#xe8ee",
                    Tags = new [] { "panel", "container" }
                },
                new Example()
                {
                    Name = "Card",
                    Path = "card",
                    Icon = "&#xe919",
                    Tags = new [] { "container" }
                },
                new Example()
                {
                    Name = "Fieldset",
                    Path = "fieldset",
                    Icon = "&#xe850",
                    Tags = new [] { "form", "container" }
                },
                new Example()
                {
                    Name = "Panel",
                    Path = "panel",
                    Icon = "&#xe14f",
                    Tags = new [] { "container" }
                },
                new Example()
                {
                    Name = "Tabs",
                    Path = "tabs",
                    Icon = "&#xe8d8",
                    Tags = new [] { "tabstrip", "tabview", "container" }
                },
                new Example()
                {
                    Name = "Steps",
                    Path = "steps",
                    Icon = "&#xe044",
                    Tags = new [] { "step", "steps", "wizard" }
                },
            }
        }};

        public static Example FindCurrent(Uri uri)
        {
            return AllExamples.SelectMany(example => example.Children ?? new[] { example })
                           .FirstOrDefault(example => example.Path == uri.AbsolutePath || $"/{example.Path}" == uri.AbsolutePath);
        }

        public static string TitleFor(Example example)
        {
            if (example != null && example.Name != "First Look")
            {
                return example.Title ?? $"Blazor {example.Name} | a free UI component by Radzen";
            }

            return "Free Blazor Components | 40+ controls by Radzen";
        }

        public static IEnumerable<Example> Filter(string term)
        {
            Func<string, bool> contains = value => value.Contains(term, StringComparison.OrdinalIgnoreCase);

            Func<Example, bool> filter = (example) => contains(example.Name) || (example.Tags != null && example.Tags.Any(contains));

            return AllExamples.Where(category => category.Children != null && category.Children.Any(filter))
                           .Select(category => new Example()
                           {
                               Name = category.Name,
                               Expanded = true,
                               Children = category.Children.Where(filter).ToArray()
                           }).ToList();
        }
    }
}
