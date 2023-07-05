using System;

namespace NSubject
{
    
    public interface ISubject { public void SubjectEffect(Weapon weapon); }
    
    public class FisherMan:ISubject
    {
        public void SubjectEffect(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }
    public class Architect:ISubject
    {
        public void SubjectEffect(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }
    public class GraveRobber:ISubject
    {
        public void SubjectEffect(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }
    public class Electrician:ISubject
    {
        public void SubjectEffect(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }
    public class Exorcist:ISubject
    {
        public void SubjectEffect(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }
    public class Farmer:ISubject
    {
        public void SubjectEffect(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }
    
}