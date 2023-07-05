using System;

namespace NEnforce
{
    public abstract class Enforce
    {
        public readonly string condition;

        public abstract bool Unlock(Weapon weapon);
            
        public abstract float SuccessPercentage(Weapon weapon);
        
        public abstract void Try (Weapon weapon);
    }
    public class Promote:Enforce
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
    public class Additional:Enforce
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
    public class MagicEngrave:Enforce
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
    public class SoulCrafting:Enforce
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
    public class Refinement:Enforce
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