﻿using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Shell;

namespace StartPagePlus.UI.ViewModels
{
    using Core.Interfaces;
    using GalaSoft.MvvmLight.Messaging;
    using Interfaces;

    public class NewsItemsViewModel : ColumnViewModel
    {
        private const string DEV_NEWS_FEED_URL = "https://vsstartpage.blob.core.windows.net/news/vs";
        private const string HEADING = "Read Developer News";

        private ObservableCollection<NewsItemViewModel> items = new ObservableCollection<NewsItemViewModel>();

        public NewsItemsViewModel(INewsItemDataService dataService, INewsItemActionService actionService, INewsItemCommandService commandService)
        {
            DataService = dataService;
            ActionService = actionService;
            CommandService = commandService;

            Heading = HEADING;
            IsVisible = true;

            GetCommands();
            Refresh();

            MessengerInstance.Register<NotificationMessage<NewsItemViewModel>>(this,
                (message) => ActionService.DoAction(message.Content));
        }

        public INewsItemDataService DataService { get; }

        public INewsItemActionService ActionService { get; }

        public INewsItemCommandService CommandService { get; }

        public ObservableCollection<NewsItemViewModel> Items
        {
            get => items;
            set => Set(ref items, value);
        }

        private void GetCommands()
            => Commands = CommandService.GetCommands(/*MoreNews,*/ Refresh);

        private void MoreNews()
        { }

        public void Refresh()
             => ThreadHelper.JoinableTaskFactory.RunAsync(async ()
                 =>
                 {
                     Items.Clear();

                     return Items = await DataService.GetItemsAsync(DEV_NEWS_FEED_URL);
                 });
    }
}