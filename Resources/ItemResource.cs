﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources
{
    class ItemResource : Resource
    {
        public static readonly ResourceType CurrentType = ResourceType.Item;
        public static readonly string CurrentVersion = "0.0.0.0";

        // Resource //
        public Subresource<InterfaceResource> Interface { get; set; } = new Subresource<InterfaceResource>();
        public Subresource<ToolResource> Tool { get; set; } = new Subresource<ToolResource>();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Stack { get; set; } = 1;
        public int Durability { get; set; } = 0;

        // Redactor //

        public ItemResource() : base(CurrentType, CurrentVersion)
        {

        }
        public ItemResource(string path) : base(path)
        {

        }
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Interface.Dispose();
                Tool.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}