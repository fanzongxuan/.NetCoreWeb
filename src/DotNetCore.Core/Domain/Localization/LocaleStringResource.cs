namespace DotNetCore.Core.Domain.Localization
{
    public class LocaleStringResource: BaseEntity
    {
        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public virtual Language Language { get; set; }
    }
}
