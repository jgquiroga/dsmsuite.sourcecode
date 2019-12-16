﻿using System.Collections.Generic;
using DsmSuite.DsmViewer.Model.Interfaces;
using DsmSuite.DsmViewer.ViewModel.Common;
using System.Windows.Input;
using System.Windows;

namespace DsmSuite.DsmViewer.ViewModel.Lists
{
    public class ElementListViewModel : ViewModelBase
    {
        public ElementListViewModel(string title, IEnumerable<IDsmElement> elements)
        {
            Title = title;

            var elementViewModels = new List<ElementListItemViewModel>();

            int index = 1;
            foreach (IDsmElement element in elements)
            {
                elementViewModels.Add(new ElementListItemViewModel(index, element));
                index++;
            }

            Elements = elementViewModels;

            CopyToClipboardCommand = new RelayCommand<object>(CopyToClipboardExecute);
        }

        public string Title { get; }

        public List<ElementListItemViewModel> Elements { get;  }

        public ICommand CopyToClipboardCommand { get; }

        private void CopyToClipboardExecute(object parameter)
        {
            Clipboard.SetText("Copy elements");
        }
    }
}
