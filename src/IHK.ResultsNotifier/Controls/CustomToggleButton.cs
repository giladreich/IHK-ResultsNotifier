using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;


namespace IHK.ResultsNotifier.Controls
{
    public sealed class CustomToggleButton : CheckBox
    {
        private IComponentChangeService componentChangeService;

        public const string BTN_START_TAG = "start";
        public const string BTN_STOP_TAG = "stop";

        private string _startText = "Start";

        private Color _colorStartBackcolor          = Color.FromArgb(128, 128, 255);
        private Color _colorStartForeColor          = Color.FromArgb(128, 255, 128);
        private Color _colorStartMouseOverBackColor = Color.FromArgb(112, 219, 112);

        private Color _colorStopBackcolor           = Color.FromArgb(245, 69, 89);
        private Color _colorStopForeColor           = Color.FromArgb(243, 122, 122);
        private Color _colorStopMouseOverBackColor  = Color.FromArgb(213, 102, 159);


        [DefaultValue("Start")]
        [Category("~ Custom Data")]
        public string StartText
        {
            get => _startText;
            set
            {
                Text = _startText = value; 
                if (DesignMode) Invalidate();
            }
        }

        [DefaultValue("Start")]
        [Category("~ Custom Data")]
        public string StopText { get; set; } = "Stop";

        [Category("~ Custom Data")]
        public bool IsActivated {
            get => Checked;
            set
            {
                Checked = value;
                if (DesignMode) Invalidate();
            }
        }

        [Category("~ Custom Data")]
        public Color ColorStartBackcolor
        {
            get => _colorStartBackcolor;
            set
            {
                BackColor = _colorStartBackcolor = value;
                if (DesignMode) Invalidate();
            }
        }

        [Category("~ Custom Data")]
        public Color ColorStartForeColor
        {
            get => _colorStartForeColor;
            set
            {
                ForeColor = _colorStartForeColor = value;
                if (DesignMode) Invalidate();
            }
        }

        [Category("~ Custom Data")]
        public Color ColorStartMouseOverBackColor
        {
            get => _colorStartMouseOverBackColor;
            set
            {
                FlatAppearance.MouseOverBackColor = _colorStartMouseOverBackColor = value;
                if (DesignMode) Invalidate();
            }
        }

        [Category("~ Custom Data")]
        public Color ColorStopBackcolor
        {
            get => _colorStopBackcolor;
            set
            {
                _colorStopBackcolor = value;
                if (DesignMode) Invalidate();
            }
        }

        [Category("~ Custom Data")]
        public Color ColorStopForeColor
        {
            get => _colorStopForeColor;
            set
            {
                _colorStopForeColor = value;
                if (DesignMode) Invalidate();
            }
        }

        [Category("~ Custom Data")]
        public Color ColorStopMouseOverBackColor
        {
            get => _colorStopMouseOverBackColor;
            set
            {
                _colorStopMouseOverBackColor = value; 
                if (DesignMode) Invalidate();
            }
        }

        public override ISite Site
        {
            get => base.Site;
            set => OverridenSite(value);
        }


        public CustomToggleButton()
        {
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BackColor = Color.FromArgb(128, 128, 255);
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseOverBackColor = Color.FromArgb(112, 219, 112);
            FlatStyle = FlatStyle.Flat;
            Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(128, 255, 128);
            Size = new Size(109, 46);
            Tag = BTN_START_TAG;
            Text = "Start";
            UseVisualStyleBackColor = false;
            Appearance = Appearance.Button;
            TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            if (Checked)
                Stop();
            else
                Start();

            base.OnCheckedChanged(e);
        }

        private void Start()
        {
            Text = StartText;
            Tag = BTN_START_TAG;
            BackColor = ColorStartBackcolor;
            ForeColor = ColorStartForeColor;
            FlatAppearance.MouseOverBackColor = ColorStartMouseOverBackColor;
        }

        private void Stop()
        {
            Text = StopText;
            Tag = BTN_STOP_TAG;
            BackColor = ColorStopBackcolor;
            ForeColor = ColorStopForeColor;
            FlatAppearance.MouseOverBackColor = ColorStopMouseOverBackColor;
        }

        private void OverridenSite(ISite site)
        {
            componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (componentChangeService != null)
                componentChangeService.ComponentChanged -= OnComponentChanged;
            base.Site = site;
            if (!DesignMode)
                return;
            componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (componentChangeService != null)
                componentChangeService.ComponentChanged += OnComponentChanged;
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
        {
            if (!(ce.Component is CustomToggleButton btn) || !btn.DesignMode)
                return;
            if (((IComponent)ce.Component).Site == null || ce.Member == null 
                                                        || ce.Member.Name != "Text")
                return;
            if (btn.Text == btn.Name)
                btn.Text = btn.Name.Replace("customToggleButton", StartText);
        }

    }
}
