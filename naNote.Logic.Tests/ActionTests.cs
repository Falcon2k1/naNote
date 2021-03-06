﻿using naNote.Logic.Data;
using naNote.Logic.Model;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace naNote.Logic.Tests
{
    public class ActionTests
    {
        [Fact]
        public void DiaryTest()
        {
            //Assign
            Catalog catalog = new Catalog();

            //Act
            catalog.AddDiary(new HashSet<int>() { 0, 1, 2 }, "Diary #1");
            catalog.AddDiary(new HashSet<int>() { 4, 5, 6 }, "Diary #2");

            //Assert
            Assert.Contains("Diary #1", catalog.DiaryList.First().Entries.First());
            Assert.Equal(new HashSet<int>() {0,1,2,4,5,6}, catalog.DiaryList.Last().CategoryIDs);

        }
        [Fact]
        public void NoteTest()
        {
            //Assign
            Catalog catalog = new Catalog();

            //Act
            catalog.AddNote(new HashSet<int>() { 0, 1, 2 }, "Note #1");
            catalog.AddNote(new HashSet<int>() { 4, 5, 6 }, "Note #2");

            //Assert
            Assert.Equal("Note #1", catalog.NoteList.First().Entry);
            Assert.Equal(new List<int>() { 4, 5, 6 }, catalog.NoteList.Last().CategoryIDs);
        }
    }
}
