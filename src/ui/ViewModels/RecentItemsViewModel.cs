﻿using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace StartPagePlus.UI.ViewModels
{
    using Interfaces;

    public class RecentItemsViewModel : ColumnViewModel
    {
        private const string HEADING = "Open a Recent Item";

        private ObservableCollection<RecentItemViewModel> items = new ObservableCollection<RecentItemViewModel>();
        //private readonly bool filtered = true;

        public RecentItemsViewModel(IRecentItemDataService dataService, IRecentItemActionService actionService, IRecentItemCommandService commandService)
            : base()
        {
            DataService = dataService;
            ActionService = actionService;
            CommandService = commandService;

            Heading = HEADING;
            IsVisible = true;

            GetCommands();
            // refresh needs to be done in the view's code-behind

            MessengerInstance.Register<NotificationMessage<RecentItemViewModel>>(this, ActionCallback);
        }

        public IRecentItemDataService DataService { get; }

        public IRecentItemActionService ActionService { get; }

        public IRecentItemCommandService CommandService { get; }

        public ObservableCollection<RecentItemViewModel> Items
        {
            get => items;
            set => Set(ref items, value);
        }

        //public bool Filtered
        //{
        //    get => filtered;
        //    set => Set(ref filtered, value);
        //}

        private void ActionCallback(NotificationMessage<RecentItemViewModel> message)
            => ActionService.ExecuteAction(message.Content);

        private void GetCommands()
            => Commands = CommandService.GetCommands(/*ShowFilter, RemoveFilter,*/ Refresh);

        //private void ShowFilter()
        //{
        //    Filtered = true;
        //    Commands[0].IsVisible = false;
        //    Commands[1].IsVisible = true;
        //}

        //private void RemoveFilter()
        //{
        //    Filtered = false;
        //    Commands[1].IsVisible = false;
        //    Commands[0].IsVisible = true;
        //}

        public void Refresh()
        {
            Items.Clear();
            Items = DataService.GetItems();
        }
    }
}