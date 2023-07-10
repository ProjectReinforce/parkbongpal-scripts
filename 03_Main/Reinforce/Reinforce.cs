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