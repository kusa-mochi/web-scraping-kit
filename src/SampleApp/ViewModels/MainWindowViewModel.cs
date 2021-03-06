﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;
using WebScrapingKit;

namespace SampleApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 変更通知プロパティ

        private string _urls;
        public string Urls
        {
            get { return _urls; }
            set
            {
                _urlList = value.Split(';');
                SetProperty(ref _urls, value);
            }
        }

        private string _xpath;
        public string XPath
        {
            get { return _xpath; }
            set { SetProperty(ref _xpath, value); }
        }

        private ObservableCollection<string> _result = new ObservableCollection<string>();
        public ObservableCollection<string> Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        private bool _IsStartButtonEnabled = true;
        public bool IsStartButtonEnabled
        {
            get { return _IsStartButtonEnabled; }
            set { SetProperty(ref _IsStartButtonEnabled, value); }
        }

        private DelegateCommand _StartCommand;
        public DelegateCommand StartCommand =>
            _StartCommand ?? (_StartCommand = new DelegateCommand(ExecuteStartCommand));

        #endregion

        #region データメンバ

        private string[] _urlList = null;

        #endregion

        #region コマンド

        private async void ExecuteStartCommand()
        {
            IsStartButtonEnabled = false;
            var webScraper = new HtmlDataGetter();
            var progress = new Progress<HtmlData>((result) =>
            {
                switch (result.Result)
                {
                    case HtmlDataGetterResult.Success:
                        break;
                    case HtmlDataGetterResult.FinalDataSuccess:
                        IsStartButtonEnabled = true;
                        break;
                    default:
                        return;
                }

                Result.AddRange(result.Data);
            });
            var cancelSrc = new CancellationTokenSource();

            try
            {
                await webScraper.GetDataFromHtmlAsync(
                    _urlList,
                    XPath,
                    progress,
                    cancelSrc.Token
                    );
            }
            catch (OperationCanceledException)
            {
                IsStartButtonEnabled = true;
                return;
            }
        }

        #endregion

        #region コンストラクタ

        public MainWindowViewModel()
        {
        }

        #endregion
    }
}
