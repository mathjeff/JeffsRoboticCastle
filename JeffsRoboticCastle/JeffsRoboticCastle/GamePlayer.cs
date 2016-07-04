using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// A GamePlayer is the representation of the representation of the Player inside the storyline
// A LevelPlayer is the representation of the Player inside the World
// Essentially the difference is that a LevelPlayer has a location and things like that
namespace Castle
{
    class GamePlayer
    {
        public GamePlayer(double hitpoints)
        {
            this.Hitpoints = hitpoints;
        }
        // makes a new LevelPlayer ready for playing
        public LevelPlayer PrepareForNewLevel()
        {
            List<Weapon> weapons = this.makeWeapons();
            LevelPlayer player = new LevelPlayer(this, new double[]{200, 100}, weapons);
            return player;
        }

        public double Hitpoints;
        
        public void AddPatron(Patron patron)
        {
            this.patrons.Add(patron);
        }
        public IEnumerable<Patron> GetPatrons()
        {
            return this.patrons;
        }
        public void AddWeaponAugment(WeaponAugment weaponAugment)
        {
            WeaponAugmentTemplate key = weaponAugment.Template;
            if (!this.weaponAugments.ContainsKey(key)) {
                this.weaponAugments[key] = new List<WeaponAugment>();
            }
            this.weaponAugments[key].Add(weaponAugment);
        }

        private List<Weapon> makeWeapons()
        {
            List<Weapon> weapons = new List<Weapon>();
            foreach (WeaponConfiguration configuration in this.weaponConfigurations)
            {
                weapons.Add(configuration.MakeWeapon());
            }
            return weapons;
        }
        public List<WeaponConfiguration> WeaponConfigurations
        {
            get
            {
                return this.weaponConfigurations;
            }
        }
        public Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> WeaponAugments
        {
            get
            {
                return this.weaponAugments;
            }
        }
        public List<WeaponAugment> GetUnassignedWeaponAugments(WeaponAugmentTemplate template)
        {
            List<WeaponAugment> augments = new List<WeaponAugment>();
            if (!this.weaponAugments.ContainsKey(template))
                return augments;
            foreach (WeaponAugment augment in this.weaponAugments[template])
            {
                if (augment.AssignedWeapon == null)
                    augments.Add(augment);
            }
            return augments;
        }
        public Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> GetUnassignedWeaponAugments()
        {
            Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> augments = new Dictionary<WeaponAugmentTemplate, List<WeaponAugment>>();
            foreach (WeaponAugmentTemplate template in this.weaponAugments.Keys)
            {
                List<WeaponAugment> unassignedOfThisType = this.GetUnassignedWeaponAugments(template);
                if (unassignedOfThisType.Count > 0)
                    augments[template] = unassignedOfThisType;
            }
            return augments;
        }
        public Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> GetAugmentsAssignableTo(WeaponConfiguration weaponConfiguration)
        {
            Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> augments = new Dictionary<WeaponAugmentTemplate, List<WeaponAugment>>();
            foreach (WeaponAugmentTemplate template in this.weaponAugments.Keys)
            {
                List<WeaponAugment> assignableOfThisType = new List<WeaponAugment>();
                foreach (WeaponAugment augment in this.weaponAugments[template])
                {
                    if (augment.AssignedWeapon == null || augment.AssignedWeapon == weaponConfiguration)
                        assignableOfThisType.Add(augment);
                }
                if (assignableOfThisType.Count > 0)
                    augments[template] = assignableOfThisType;
            }
            return augments;
        }


        List<Patron> patrons = new List<Patron>();
        Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> weaponAugments = new Dictionary<WeaponAugmentTemplate, List<WeaponAugment>>();
        List<WeaponConfiguration> weaponConfigurations = new List<WeaponConfiguration>();
    }
}
