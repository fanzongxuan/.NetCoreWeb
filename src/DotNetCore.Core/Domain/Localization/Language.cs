using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Localization
{
    public class Language: BaseEntity
    {
        private ICollection<LocaleStringResource> _localeStringResources;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Name { get; set; }

        /// <summary>
        /// Gets or sets Culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the langage is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets display order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets locale string resources
        /// </summary>
        public virtual ICollection<LocaleStringResource> LocaleStringResources
        {
            get { return _localeStringResources ?? (_localeStringResources = new List<LocaleStringResource>()); }
            protected set { _localeStringResources = value; }
        }
    }
}
