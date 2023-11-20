using System;

namespace Auctus.DataMiner.Library.Auctus.Common.Models
{
    /// <summary>
    /// Model used in conjunction with the GetAvailableDocuments SLNet call.
    /// </summary>
    public class DmaDocument
    {
        /// <summary>Initializes a new instance of the <see cref="DmaDocument" /> class.</summary>
        /// <param name="name">The name and extension of the document.</param>
        /// <param name="description">The description of the document.</param>
        /// <param name="comments">The comments associated with the document.</param>
        /// <param name="date">The Date/Time that the document was last modified.</param>
        /// <param name="isHyperlink">Flag indicating if the document is a hyperlink.</param>
        /// <param name="hyperlink">The hyperlink of this document.</param>
        public DmaDocument(string name, string description, string comments, DateTime date, bool isHyperlink, string hyperlink)
        {
            Name = name;
            Description = description;
            Comments = comments;
            Date = date;
            IsHyperlink = isHyperlink;
            Hyperlink = hyperlink;
        }

        /// <summary>
        /// The name and extension of the document.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description of the document.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The comments associated with the document.
        /// </summary>
        public string Comments { get; }

        /// <summary>
        /// The Date/Time that the document was last modified.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Flag indicating if the document is a hyperlink.
        /// </summary>
        public bool IsHyperlink { get; }

        /// <summary>
        /// The hyperlink of this document.
        /// </summary>
        public string Hyperlink { get; set; }
    }
}