//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABC
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccessLevelUser
    {
        public int id { get; set; }
        public int accesslevelid { get; set; }
        public int userid { get; set; }
    
        public virtual AccessLevel AccessLevel { get; set; }
        public virtual UserData UserData { get; set; }
    }
}
