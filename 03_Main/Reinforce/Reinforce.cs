using System;

namespace NEnforce
{
    public abstract class Reinforce
    {
        public readonly string condition;

        public abstract bool Unlock(Weapon weapon);
            
        public abstract float SuccessPercentage(Weapon weapon);
        
        public abstract void Try (Weapon weapon);
    }
    public class Promote:Reinforce
    {
        private string condition;

        public override bool Unlock(Weapon weapon)
        {
            return true;
        }

        public override float SuccessPercentage(Weapon weapon)
        {
            return 0;
        }

        public override void Try(Weapon weapon)
        {
            
        }
    }
    public class Additional:Reinforce
    {
        private string condition;

        public override bool Unlock(Weapon weapon)
        {
            return true;
        }

        public override float SuccessPercentage(Weapon weapon)
        {
            return 0;
        }

        public override void Try(Weapon weapon)
        {
            
        }
    }
    public class MagicEngrave:Reinforce
    {
        private string condition;

        public override bool Unlock(Weapon weapon)
        {
            return true;
        }

        public override float SuccessPercentage(Weapon weapon)
        {
            return 0;
        }

        public override void Try(Weapon weapon)
        {
            
        }
    }
    public class SoulCrafting:Reinforce
    {
        private string condition;

        public override bool Unlock(Weapon weapon)
        {
            return true;
        }

        public override float SuccessPercentage(Weapon weapon)
        {
            return 0;
        }

        public override void Try(Weapon weapon)
        {
            
        }
    }
    public class Refinement:Reinforce
    {
        private string condition;

        public override bool Unlock(Weapon weapon)
        {
            return true;
        }

        public override float SuccessPercentage(Weapon weapon)
        {
            return 0;
        }

        public override void Try(Weapon weapon)
        {
            
        }
    }
    
    
}