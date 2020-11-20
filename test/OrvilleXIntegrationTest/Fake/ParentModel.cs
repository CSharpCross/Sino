using System;
using System.Collections.Generic;
using System.Text;

namespace OrvilleXIntegrationTest.Fake
{
    public class ParentModel
    {
        public ChildModel Child { get; set; } = new ChildModel();
    }
}
