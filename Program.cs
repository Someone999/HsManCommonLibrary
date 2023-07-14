using HsManCommonLibrary.Timers;

namespace HsManCommonLibrary;

class Program
{
    public static bool IsMatch(string s, string p) {
        if(p == ".*")
        {
            return true;
        }

        if(s.Length == 0)
        {
            return false;
        }
        
        int j = 0;
        for(int i = 0; i < p.Length; i++)
        {
            
            if(p[i] == '.')
            {
                j++;
                continue;
            }

            if(p[i] == '*')
            {
                if(i == 0)
                {
                    return false;
                }

                char lastChar = p[i - 1];
                while(true)
                {
                    if(s[j] == lastChar || lastChar == '.')
                    {
                        j++;
                    }
                    else
                    {
                        break;
                    }

                    if(j > s.Length - 1)
                    {
                        break;
                    }
                }
                

                continue;
            }

            if (j >= s.Length)
            {
                return i == p.Length;
            }

            if(p[i] == s[j])
            {
                j++;
            }
        }

        return j == s.Length;
    }
    static void Main(string[] args)
    {
        var x = IsMatch("ab", ".*c");
    }
}