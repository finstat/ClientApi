extern alias CZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinstatApi.ViewModel;

namespace DesktopFinstatApiTester.ViewModel
{
    public class Limit : ViewModel, IModelViewModel<FinstatApi.ViewModel.Limit>, IModelViewModel<CZ::FinstatApi.ViewModel.Limit>
    {
        public const string CurrentProperty = "Current";
        public const string MaxProperty = "Max";
        public const string LimitObjectProperty = "LimitObject";

        public Limit()
        {
            PropertyChanged += Limit_PropertyChanged;
        }

        private long _current;
        public long Current
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    RaisePropertyChanged(CurrentProperty);
                }
            }
        }

        private long _max;
        public long Max
        {
            get { return _max; }
            set
            {
                if (_max != value)
                {
                    _max = value;
                    RaisePropertyChanged(MaxProperty);
                }
            }
        }


        private void Limit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (new[] { CurrentProperty, MaxProperty }.Contains(e.PropertyName))
            {
                this.RaisePropertyChanged(LimitObjectProperty);
            }
        }

        public void FromModel(FinstatApi.ViewModel.Limit model)
        {
            if (model != null)
            {
                Current = model.Current;
                Max = model.Max;
            }
        }

        public FinstatApi.ViewModel.Limit ToModel(FinstatApi.ViewModel.Limit model)
        {
            if (model == null)
            {
                model = new FinstatApi.ViewModel.Limit();
            }
            model.Current = Current;
            model.Max = Max;
            return model;
        }

        public void FromModel(CZ::FinstatApi.ViewModel.Limit model)
        {
            if (model != null)
            {
                Current = model.Current;
                Max = model.Max;
            }
        }

        public CZ::FinstatApi.ViewModel.Limit ToModel(CZ::FinstatApi.ViewModel.Limit model)
        {
            if(model == null)
            {
                model = new CZ::FinstatApi.ViewModel.Limit();
            }
            model.Current = Current;
            model.Max = Max;
            return model;
        }
    }

    public class Limits : ViewModel, IModelViewModel<FinstatApi.ViewModel.Limits>, IModelViewModel<CZ::FinstatApi.ViewModel.Limits>
    {
        public const string DailyProperty = "Daily";
        public const string MonthlyProperty = "Monthly";
        public const string LimitsObjectProperty = "LimitsObject";

        public Limits()
        {
            PropertyChanged += Limits_PropertyChanged;
            Daily = new Limit();
            Monthly = new Limit();
        }

        private Limit _daily;
        public Limit Daily
        {
            get { return _daily; }
            set
            {
                if (_daily != value)
                {
                    if(_daily != null)
                    {
                        _daily.PropertyChanged -= _daily_PropertyChanged;
                    }
                    _daily = value;
                    if (_daily != null)
                    {
                        _daily.PropertyChanged += _daily_PropertyChanged;
                    }
                }
            }
        }


        private Limit _monthly;
        public Limit Monthly
        {
            get { return _monthly; }
            set
            {
                if (_monthly != value)
                {
                    if (_monthly != null)
                    {
                        _monthly.PropertyChanged -= _monthly_PropertyChanged;
                    }
                    _monthly = value;
                    if (_monthly != null)
                    {
                        _monthly.PropertyChanged += _monthly_PropertyChanged;
                    }
                }
            }
        }

        private void _monthly_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Limit.LimitObjectProperty)
            {
                RaisePropertyChanged(MonthlyProperty);
            }
        }

        private void _daily_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Limit.LimitObjectProperty)
            {
                RaisePropertyChanged(DailyProperty);
            }
        }

        private void Limits_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (new[] { DailyProperty, MonthlyProperty }.Contains(e.PropertyName))
            {
                this.RaisePropertyChanged(LimitsObjectProperty);
            }
        }

        public void FromModel(FinstatApi.ViewModel.Limits model)
        {
            if (model != null)
            {
                Daily.FromModel(model?.Daily);
                Monthly.FromModel(model.Monthly);
            }
        }

        public FinstatApi.ViewModel.Limits ToModel(FinstatApi.ViewModel.Limits model)
        {
            if (model == null)
            {
                model = new FinstatApi.ViewModel.Limits();
            }
            model.Daily = Daily.ToModel(new FinstatApi.ViewModel.Limit());
            model.Monthly = Monthly.ToModel(new FinstatApi.ViewModel.Limit());
            return model;
        }

        public void FromModel(CZ::FinstatApi.ViewModel.Limits model)
        {
            if(model != null)
            {
                Daily.FromModel(model?.Daily);
                Monthly.FromModel(model.Monthly);
            }
        }

        public CZ::FinstatApi.ViewModel.Limits ToModel(CZ::FinstatApi.ViewModel.Limits model)
        {
            if (model == null)
            {
                model = new CZ::FinstatApi.ViewModel.Limits();
            }
            model.Daily = Daily.ToModel(new CZ::FinstatApi.ViewModel.Limit());
            model.Monthly = Monthly.ToModel(new CZ::FinstatApi.ViewModel.Limit());
            return model;
        }
    }
}
