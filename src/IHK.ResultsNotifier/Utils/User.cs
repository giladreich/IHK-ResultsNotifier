using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHK.ResultsNotifier.Utils
{
    public class User : IEquatable<User>, IComparable<User>
    {

        public string Username { get; }
        public string Password { get; }


        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public User(User other) 
            : this(other.Username, other.Password)
        { }



        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((User) obj);
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return String.Equals(Username, other.Username) 
                && String.Equals(Password, other.Password);
        }

        public int CompareTo(User other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int usernameComparison = String.Compare(Username, other.Username, StringComparison.Ordinal);
            if (usernameComparison != 0) return usernameComparison;

            return String.Compare(Password, other.Password, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int userHash = Username != null ? Username.GetHashCode() : 0;
                int passHash = Password != null ? Password.GetHashCode() : 0;
                int hashCode = (userHash * 397) ^ passHash;

                return hashCode;
            }
        }

        public override string ToString()
        {
            return Username;
        }
    }
}
