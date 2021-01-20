﻿using naNote.Logic.Data;
using naNote.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using LoremGenerator;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace naNote.CMD
{
    internal class CMDActions
    {
        //TODO: Add a tab-completion helper thing here.
        public static string Act(string action, Catalog catalog, HashSet<int> categories, string payload)
        {
            switch (action.ToLower())
            {
                case "diary":
                    catalog.AddDiary(categories, payload);
                    return "diary updated!";

                case "note":
                    catalog.AddNote(categories, payload);
                    return "note added!";

                case "reminder":
                    return "reminder! Not yet implemented though.";

                case "list":
                    // TODO: Can I replace this with a yield return statement instead of using Linq?
                    // Seems like it would be more effective but I'd need to figure it out.

                    var returnTake = 10;
                    var returnSkip = 0;
                    var validOptions = new string[3] { "diary", "note", "category" };
                    if (!validOptions.Contains(payload))
                    {
                        return CMDStrings.CommandNotFound;
                    }

                    var _catalogList = BuildTextList(payload, catalog);

                    while (true)
                    {
                        foreach (var item in _catalogList.Skip(returnSkip).Take(returnTake))
                        {
                            Console.WriteLine(item);
                        }
                        Console.WriteLine("Press n to continue, or any other key to return.");
                        if (Console.ReadKey(true).KeyChar != 'n')
                        {
                            return "Returning to main menu.";
                        }
                        returnSkip += returnTake;
                    }

                case "debug":
                    var watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < 10000; i++)
                    {
                        Parser _parser = new Parser(Generator.GenerateEntry("note", 2, 10, 2, 10), catalog);
                        catalog.AddNote(_parser.Categories, _parser.Payload);
                    }
                    watch.Stop();
                    return $"Creation of 10,000 notes completed in {watch.ElapsedMilliseconds} ms";

                case "save":
                    Access.Save(catalog);
                    return "Saved!";

                case "load":
                    catalog = Access.Load();
                    return "Load complete!";
                case "quit":
                case "exit":
                    Environment.Exit(0);
                    return "You won't see this!";

                case "help":
                    var type = Type.GetType("naNote.CMD.CMDStrings");
                    var field = type?.GetField("Help"+payload);
                    var value = field?.GetValue(null);
                    if (value != null)
                    {
                        return value.ToString();
                    }
                    else
                    {
                        return CMDStrings.CommandNotFound;
                    }
                    
                default:
                    return CMDStrings.CommandNotFound;
            }
        }

        private static List<string> BuildTextList(string action, Catalog catalog)
        {
            var _returnList = new List<string>();

            switch (action.ToLower())
            {
                case "diary":
                    foreach (var diary in catalog.DiaryList.AsEnumerable().Reverse())
                    {
                        _returnList.Add(
                            $"Entry #{diary.Id}, created at {diary.CreatedDtime.ToShortDateString()}\n" +
                            $"{diary.Entry}\n" +
                            $"Categories: { GetCategoryList(diary.CategoryIDs, catalog) }");
                    }
                    break;
                case "note":
                    foreach (var note in catalog.NoteList.AsEnumerable().Reverse())
                    {
                        _returnList.Add(
                            $"Entry #{note.Id} at {note.CreatedDtime.ToShortDateString()} " +
                            $"{note.Entry}\n" +
                            $"Categories: { GetCategoryList(note.CategoryIDs, catalog)}");
                    }
                    break;
                case "category":
                    foreach (var category in catalog.CategoryList.AsEnumerable().Reverse())
                    {
                        _returnList.Add($"Entry #{category.Id} at {category.CreatedDtime.ToShortDateString()} " +
                            $"{category.Name}");
                    }
                    break;
                default:
                    _returnList.Add(CMDStrings.CommandNotFound);
                    break;
            }

            return _returnList;
        }
        private static string GetCategoryList(HashSet<int> categoryList, Catalog catalog)
        {
            var _catArr = new Queue<string>();
            foreach (var categoryID in categoryList)
            {
                string _name = catalog.CategoryList.FirstOrDefault(p => p.Id == categoryID).Name;
                _catArr.Enqueue(_name);
            }
            return string.Join(", ", _catArr);
        }
    }

    
}