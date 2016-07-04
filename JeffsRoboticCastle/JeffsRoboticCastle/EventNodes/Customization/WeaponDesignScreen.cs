using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// This screen lets the user assemble the weapon they want based on the components they have
namespace Castle.EventNodes.Customization
{
    class WeaponDesignScreen : Screen
    {
        public WeaponDesignScreen(Size screenSize, GamePlayer player)
        {
            base.Initialize(screenSize);
            this.player = player;
        }
        public void Show(WeaponConfiguration weaponConfiguration)
        {
            this.Done = false;
            this.weaponConfiguration = weaponConfiguration;
            this.getCanvas().Children.Clear();

            // TODO this screen could benefit from the VisiPlacer project too

            List<FrameworkElement> controls = new List<FrameworkElement>();

            TextBlock title = new TextBlock();
            title.Text = "Select up to " + this.weaponConfiguration.BasicWeapon.NumAugmentSlots + " augments for this weapon";
            Size size = this.getSize();
            controls.Add(title);

            this.identifyAvailableAugments();

            // Should we just use one ListView in multiselect mode?
            this.selectorViews = new List<ListView>();
            for (int i = 0; i < this.weaponConfiguration.BasicWeapon.NumAugmentSlots; i++)
            {
                ListView selector = new ListView();
                selector.SelectionMode = SelectionMode.Single;
                selector.SelectionChanged += this.ListViewSelectionChanged;
                this.selectorViews.Add(selector);
            }


            foreach (ListView view in this.selectorViews)
            {
                controls.Add(view);
            }

            Button exitButton = new Button();
            exitButton.Content = "Back";
            exitButton.Click += exitButton_Click;
            controls.Add(exitButton);

            double heightPerItem = size.Height / controls.Count;
            double y = 0;
            foreach (FrameworkElement element in controls)
            {
                this.addControl(element, 0, y, size.Width, heightPerItem);
                y += heightPerItem;
            }

            this.makeSelectionsMatchWeapon();
        }
        private void makeSelectionsMatchWeapon()
        {
            List<WeaponAugmentTemplate> currentSelections = new List<WeaponAugmentTemplate>();
            for (int i = 0; i < this.weaponConfiguration.BasicWeapon.NumAugmentSlots; i++)
            {
                ListView view = this.selectorViews[i];
                WeaponAugment augment = this.weaponConfiguration.GetAugmentAtIndex(i);
                WeaponAugmentTemplate template = null;
                if (augment != null)
                {
                    template = augment.Template;
                }
                currentSelections.Add(template);
            }
            this.updateOptions(currentSelections);
        }

        private void updateOptions(List<WeaponAugmentTemplate> currentSelections)
        {
            this.updatingOptions = true;
            // determine which augments aren't assigned
            Dictionary<WeaponAugmentTemplate, int> unassignedAugments = new Dictionary<WeaponAugmentTemplate, int>(this.availableAugments);
            foreach (WeaponAugmentTemplate template in currentSelections)
            {
                if (template != null)
                    unassignedAugments[template]--;
            }
            // Allow each listview to show its current selection, or any other available augments, or "none"
            for (int i = 0; i < currentSelections.Count; i++)
            {
                List<WeaponAugmentTemplate> augmentTemplates = new List<WeaponAugmentTemplate>(unassignedAugments.Keys);
                ListView view = this.selectorViews[i];
                view.Items.Clear();
                view.Items.Add("(None)");
                foreach (WeaponAugmentTemplate template in unassignedAugments.Keys)
                {
                    String displayValue = template.Name;
                    bool selected = (template == currentSelections[i]);
                    if (unassignedAugments[template] > 0 || selected)
                    {
                        view.Items.Add(displayValue);
                    }
                    if (selected)
                    {
                        view.SelectedItem = displayValue;
                    }
                }
            }
            this.updatingOptions = false;
        }

        public void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.finish();
        }
        public void ListViewSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!this.updatingOptions)
                this.updateOptions(this.getSelections());
        }
        private List<WeaponAugmentTemplate> getSelections()
        {
            List<WeaponAugmentTemplate> selections = new List<WeaponAugmentTemplate>();
            foreach (ListView selector in this.selectorViews)
            {
                selections.Add(this.parseSelection((String)selector.SelectedItem));
            }
            return selections;
        }
        private WeaponAugmentTemplate parseSelection(String selection)
        {
            foreach (WeaponAugmentTemplate template in this.availableAugments.Keys)
            {
                if (template.Name == selection)
                    return template;
            }
            return null;
        }
        private void identifyAvailableAugments()
        {
            this.availableAugments = new Dictionary<WeaponAugmentTemplate, int>();
            Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> templates = this.player.GetAugmentsAssignableTo(this.weaponConfiguration);
            foreach (WeaponAugmentTemplate template in templates.Keys)
            {
                this.availableAugments[template] = templates[template].Count;
            }
        }

        private void finish()
        {
            this.applyChoices();
            this.Done = true;
        }
        private void applyChoices()
        {
            // remove all augments currently assigned to this weapon
            List<WeaponAugment> previousAugments = new List<WeaponAugment>(this.weaponConfiguration.Augments);
            foreach (WeaponAugment augment in previousAugments)
            {
                augment.Unassign();
            }
            // assign all chosen augments to this weapon
            List<WeaponAugmentTemplate> selections = this.getSelections();
            foreach (WeaponAugmentTemplate template in selections )
            {
                if (template != null)
                    player.GetUnassignedWeaponAugments(template)[0].AssignTo(this.weaponConfiguration);
            }
        }

        public bool Done;
        
        private GamePlayer player;
        //private Dictionary<CheckBox, WeaponAugment> selections;
        private WeaponConfiguration weaponConfiguration;
        private List<ListView> selectorViews;
        private Dictionary<WeaponAugmentTemplate, int> availableAugments = new Dictionary<WeaponAugmentTemplate, int>();
        private bool updatingOptions;
    }
}