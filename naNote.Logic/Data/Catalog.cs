using naNote.Logic.Model;
using System.Collections.Generic;
using naNote.Logic;
using System;
using System.Linq;
using System.Collections;

namespace naNote.Logic.Data
{
    public class Catalog
    {
        public List<Category> CategoryList { get; set; }
        public List<Diary> DiaryList { get; set; }
        public List<Note> NoteList { get; set; }

        public Catalog()
        {
            CategoryList = new List<Category>();
            DiaryList = new List<Diary>();
            NoteList = new List<Note>();
        }

        public void AddDiary(HashSet<int> categories,String payload)
        {
            // Check to see if there's an existing diary already today
            if (DiaryList.Any(p => p.CreatedDtime.Date == DateTime.Today))
            {
                // If there is an existing diary, append to it.
                var diaryToEdit = DiaryList.Last(p => p.CreatedDtime.Date == DateTime.Today);
                diaryToEdit.CategoryIDs.UnionWith(categories);
                diaryToEdit.Entries.Add($"{DateTime.Now.ToShortTimeString()}: {payload}");
            }
            else
            {
                // If there isn't, make a new one.
                var newEntry = new Diary();
                newEntry.Entries.Add($"Daily diary: {DateTime.Now.ToShortDateString()}\n{DateTime.Now.ToShortTimeString()}: {payload}");
                
                newEntry.CategoryIDs.UnionWith(categories);
                DiaryList.Add(newEntry);
            }
        }

        public void AddNote(HashSet<int> categories, string payload)
        {
            var newEntry = new Note()
            {
                Entry = payload,
            };
            newEntry.CategoryIDs.UnionWith(categories);
            NoteList.Add(newEntry);
        }
    }
}
