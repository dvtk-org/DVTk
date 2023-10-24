// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.IO;

namespace System.Xml
{

    /// <summary>
    /// XmlNodeWriter builds a tree of XmlNodes based on the XmlWriter methods that are called.
    /// </summary>
    public class XmlNodeWriter : XmlWriter
    {
        XmlNode root;
        XmlNode current;
        XmlDocument owner;
        XmlAttribute ca; // current attribute (if any)

        WriteState state;

        private void Init(XmlNode root, bool clearCurrentContents)
        {
            this.root = root;
            if (clearCurrentContents)
                this.root.RemoveAll();
            if (root is XmlDocument)
            {
                owner = (XmlDocument)root;
                state = WriteState.Start;
            }
            else
            {
                owner = root.OwnerDocument;
                state = WriteState.Content;
            }
            current = root;
        }

        /// <summary>
        /// Construct XmlNodeWriter for building the content of the given root XmlElement node.
        /// </summary>
        /// <param name="root">If root contains any content it will be completely replaced by what the XmlNodeWriter produces.</param>
        /// <param name="clearCurrentContents">Clear the current children of the given node</param>
        public XmlNodeWriter(XmlElement root, bool clearCurrentContents)
        {
            Init(root, clearCurrentContents);
        }

        /// <summary>
        /// Construct XmlNodeWriter for building the content of the given root XmlDocument node.
        /// </summary>
        /// <param name="root">If root contains any content it will be completely replaced by what the XmlNodeWriter produces.</param>
        /// <param name="clearCurrentContents">Clear the current children of the given node</param>
        public XmlNodeWriter(XmlDocument root, bool clearCurrentContents)
        {
            Init(root, clearCurrentContents);
        }

        /// <summary>
        /// Return the current state of the writer.
        /// </summary>
        public override WriteState WriteState
        {
            get
            {
                switch (state)
                {
                    case WriteState.Start:
                        return WriteState.Start;
                    case WriteState.Prolog:
                        return WriteState.Prolog;
                    case WriteState.Element:
                        return WriteState.Element;
                    case WriteState.Attribute:
                        return WriteState.Attribute;
                    case WriteState.Content:
                        return WriteState.Content;
                    case WriteState.Closed:
                        return WriteState.Closed;
                }
                return WriteState.Closed;
            }
        }

        /// <summary>
        /// Return the current XmlLang state.  This does not have an efficient implementation, so use at your own risk.
        /// </summary>
        public override string XmlLang
        {
            get
            {
                XmlNode p = current;
                while (p != null)
                {
                    XmlElement e = p as XmlElement;
                    if (e != null)
                    {
                        string s = e.GetAttribute("lang", "http://www.w3.org/XML/1998/namespace");
                        if (s != null && s != string.Empty)
                        {
                            return s;
                        }
                    }
                    p = p.ParentNode;
                }
                return null;
            }
        }
        /// <summary>
        /// Return the current XmlSpace state.  This does not have an efficient implementation, so use at your own risk.
        /// </summary>

        public override XmlSpace XmlSpace
        {
            get
            {
                XmlNode p = current;
                while (p != null)
                {
                    XmlElement e = p as XmlElement;
                    if (e != null)
                    {
                        string s = e.GetAttribute("space", "http://www.w3.org/XML/1998/namespace");
                        if (s == "default")
                        {
                            return XmlSpace.Default;
                        }
                        else if (s == "preserve")
                        {
                            return XmlSpace.Preserve;
                        }
                    }
                    p = p.ParentNode;
                }
                return XmlSpace.None;
            }
        }
        /// <summary>
        /// This auto-closes any open elements and puts the writer in the WriteState.Closed state.
        /// </summary>
        public override void Close()
        {
            current = root;
            state = WriteState.Closed;
        }
        /// <summary>
        /// This is a noop.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Returns the result of GetPrefixOfNamespace on the current node.
        /// </summary>
        /// <param name="ns">The namespaceURI to lookup the associated prefix for.</param>
        /// <returns>The prefix or <see langword="null"/> if no matching namespaceURI is in scope.</returns>
        public override string LookupPrefix(string ns)
        {
            /* applied fixes from www.gotdotnet.com
            return current.GetPrefixOfNamespace(ns);
            */
            string prefix = current.GetPrefixOfNamespace(ns);
            if (prefix == string.Empty &&
                current.GetNamespaceOfPrefix(prefix) != ns)
            {
                return null;
            }
            return prefix;
        }

        /// <summary>
        /// This method is implemented using Convert.ToBase64String.
        /// </summary>
        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            WriteString(Convert.ToBase64String(buffer, index, count));
        }

        /// <summary>
        /// This is implementd using a temporary XmlTextWriter to turn the 
        /// given binary blob into a string, then it calls WriteString with
        /// the result.
        /// </summary>
        public override void WriteBinHex(byte[] buffer, int index, int count)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter w = new XmlTextWriter(sw);
            w.WriteBinHex(buffer, index, count);
            w.Close();
            WriteString(sw.ToString());
        }

        /// <summary>
        /// Creates a System.Xml.XmlCDataSection node.
        /// </summary>
        public override void WriteCData(string text)
        {
            if (state == WriteState.Attribute || state == WriteState.Element)
                state = WriteState.Content;
            if (state != WriteState.Content)
                throw new InvalidOperationException("Writer is in the state '" + this.WriteState.ToString() + "' which is not valid for writing CData elements");
            current.AppendChild(owner.CreateCDataSection(text));
        }

        /// <summary>
        /// Writes the given char as a string.  The XmlDocument has no representation for 
        /// character entities, so the fact that this was called will be lost.
        /// </summary>
        public override void WriteCharEntity(char ch)
        {
            WriteString(Convert.ToString(ch));
        }

        /// <summary>
        /// Calls WriteString with new string(buffer, index, count).
        /// </summary>
        public override void WriteChars(char[] buffer, int index, int count)
        {
            WriteString(new string(buffer, index, count));
        }

        /// <summary>
        /// Creates an System.Xml.XmlComment node.
        /// </summary>
        public override void WriteComment(string text)
        {
            if (state == WriteState.Attribute || state == WriteState.Element)
                state = WriteState.Content;
            if (state != WriteState.Content && state != WriteState.Prolog && state != WriteState.Start)
                throw new InvalidOperationException("Writer is in the state '" + this.WriteState.ToString() + "' which is not valid for writing comments");
            current.AppendChild(owner.CreateComment(text));
            if (state == WriteState.Start) state = WriteState.Prolog;
        }

        /// <summary>
        /// Creates an System.Xml.XmlDocumentType node.
        /// </summary>
        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            if (state != WriteState.Prolog && state != WriteState.Start)
                throw new InvalidOperationException("Writer is not in the Start or Prolog state, or root node is not an XmlDocument object");
            if (owner.DocumentType != null)
                owner.RemoveChild(owner.DocumentType);
            owner.XmlResolver = null;
            current.AppendChild(owner.CreateDocumentType(name, pubid, sysid, subset));
            state = WriteState.Prolog;
        }

        /// <summary>
        /// Closes the previous WriteStartAttribute call.
        /// </summary>
        public override void WriteEndAttribute()
        {
            if (state != WriteState.Attribute)
                throw new InvalidOperationException("Writer is not in the Attribute state");
            state = WriteState.Element;
        }
        /// <summary>
        /// Closes any open elements and puts the writer back in the Start state.
        /// </summary>
        public override void WriteEndDocument()
        {
            current = root;
            state = WriteState.Start;
        }
        /// <summary>
        /// Closes the previous WriteStartElement call.
        /// </summary>
        public override void WriteEndElement()
        {
            if (current == root)
                throw new InvalidOperationException("Too many WriteEndElement calls have been made");
            current = current.ParentNode;
            state = WriteState.Content;
        }
        /// <summary>
        /// Creates a System.Xml.XmlEntityReference node.
        /// </summary>
        /// <param name="name">The name of the entity reference</param>
        public override void WriteEntityRef(string name)
        {
            if (state == WriteState.Element)
                state = WriteState.Content;
            XmlNode n = current;
            if (state == WriteState.Attribute)
            {
                n = ca;
            }
            else if (state != WriteState.Content)
            {
                throw new InvalidOperationException("Invalid state '" + WriteState.ToString() + "' for entity reference");
            }
            n.AppendChild(owner.CreateEntityReference(name));
        }

        /// <summary>
        /// The DOM does not preserve this information, so this is equivalent to WriteEndElement.
        /// </summary>
        public override void WriteFullEndElement()
        {
            this.WriteEndElement();
        }
        /// <summary>
        /// Calls WriteString if the name is a valid XML name.
        /// </summary>
        public override void WriteName(string name)
        {
            WriteString(XmlConvert.VerifyName(name));
        }
        /// <summary>
        /// Calls WriteString if the name is a valid XML NMTOKEN.
        /// </summary>
        public override void WriteNmToken(string name)
        {
            // NmToken is the same as NcName, except it doesn't restrict first character.
            string temp = XmlConvert.VerifyName("a" + name);
            WriteString(temp.Substring(1));
        }
        /// <summary>
        /// Creates a System.Xml.XmlProcessingInstruction node.
        /// </summary>
        public override void WriteProcessingInstruction(string name, string text)
        {
            if (state == WriteState.Attribute || state == WriteState.Element)
                state = WriteState.Content;
            if (state != WriteState.Content && state != WriteState.Prolog && state != WriteState.Start)
                throw new InvalidOperationException("Writer is in the state '" + this.WriteState.ToString() + "' which is not valid for writing processing instructions");
            if (name == "xml")
            {
                XmlDocument doc2 = new XmlDocument();
                doc2.InnerXml = "<?xml " + text + "?><root/>";
                current.AppendChild(owner.ImportNode(doc2.FirstChild, true));
            }
            else
            {
                current.AppendChild(owner.CreateProcessingInstruction(name, text));
            }
            if (state == WriteState.Start) state = WriteState.Prolog;
        }
        /// <summary>
        /// Looks up the prefix in scope for the given namespace and calls WriteString
        /// with the prefix+":"+localName (or just localName if the prefix is the empty string).
        /// </summary>
        public override void WriteQualifiedName(string localName, string ns)
        {
            string prefix = this.LookupPrefix(ns);
            if (prefix == null) throw new InvalidOperationException("Namespace '" + ns + "' is not in scope");
            if (prefix == string.Empty)
            {
                WriteString(localName);
            }
            else
            {
                WriteString(prefix + ":" + localName);
            }
        }
        /// <summary>
        /// WriteRaw writes out the given string "unescaped", in other words it better be well formed XML markup.
        /// So for the XmlNodeWriter we parse this string and build the resulting tree, so it maps to setting the
        /// InnerXml property.  
        /// </summary>
        /// <param name="data"></param>
        public override void WriteRaw(string data)
        {
            if (data.IndexOf("<") < 0)
            {
                WriteString(data);
                return;
            }

            switch (state)
            {
                case WriteState.Start:
                    goto case WriteState.Content;
                case WriteState.Prolog:
                    goto case WriteState.Content;
                case WriteState.Element:
                    state = WriteState.Content;
                    goto case WriteState.Content;
                case WriteState.Attribute:
                    {
                        ArrayList saved = new ArrayList();
                        if (ca.HasChildNodes)
                        {
                            while (ca.FirstChild != null)
                            {
                                saved.Add(ca.FirstChild);
                                ca.RemoveChild(ca.FirstChild);
                            }
                        }
                        ca.InnerXml = data;
                        for (int i = saved.Count - 1; i >= 0; i--)
                        {
                            ca.PrependChild((XmlNode)saved[i]);
                        }
                    }
                    break;
                case WriteState.Content:
                    {
                        ArrayList saved = new ArrayList();
                        if (current.HasChildNodes)
                        {
                            while (current.FirstChild != null)
                            {
                                saved.Add(current.FirstChild);
                                current.RemoveChild(current.FirstChild);
                            }
                        }
                        current.InnerXml = data;
                        for (int i = saved.Count - 1; i >= 0; i--)
                        {
                            current.PrependChild((XmlNode)saved[i]);
                        }
                        state = WriteState.Content;
                    }
                    break;
                case WriteState.Closed:
                    throw new InvalidOperationException("Writer is closed");
            }

        }
        /// <summary>
        /// Calls WriteRaw(string) with new string(buffer, index, count)
        /// </summary>
        public override void WriteRaw(char[] buffer, int index, int count)
        {
            WriteRaw(new string(buffer, index, count));
        }
        /// <summary>
        /// Creates a System.Xml.XmlAttribute node.
        /// </summary>
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            /* applied fixes from www.gotdotnet.com
            if (state == WriteState.Attribute)
                state = WriteState.Element;
            if (state != WriteState.Element) 
                throw new InvalidOperationException("Writer is not in a start tag, so it cannot write attributes.");
    
            ca = owner.CreateAttribute(prefix, localName, ns);
            current.Attributes.Append(ca);
            state = WriteState.Attribute;
            */
            if (state == WriteState.Attribute)
                state = WriteState.Element;
            if (state != WriteState.Element)
                throw new InvalidOperationException("Writer is not in a start tag, so it cannot write attributes.");

            if (prefix == "xmlns" && ns == null)
            {
                ns = "http://www.w3.org/2000/xmlns/";
            }
            ca = owner.CreateAttribute(prefix, localName, ns);
            current.Attributes.Append(ca);
            state = WriteState.Attribute;
        }
        /// <summary>
        /// Writes the XmlDeclaration node with a standalone attribute.  This is only allowed when the
        /// writer is in the Start state, which only happens if the writer was constructed with an
        /// XmlDocument object.
        /// </summary>
        public override void WriteStartDocument()
        {
            if (state != WriteState.Start)
                throw new InvalidOperationException("Writer is not in the Start state or root node is not an XmlDocument object");
            current.AppendChild(owner.CreateXmlDeclaration("1.0", null, null));
            state = WriteState.Prolog;
        }
        /// <summary>
        /// Writes the XmlDeclaration node with a standalone attribute.  This is only allowed when the
        /// writer is in the Start state, which only happens if the writer was constructed with an
        /// XmlDocument object.
        /// </summary>
        /// <param name="standalone">If true, standalone attribute has value "yes" otherwise it has the value "no".</param>
        public override void WriteStartDocument(bool standalone)
        {
            if (state != WriteState.Start)
                throw new InvalidOperationException("Writer is not in the Start state or root node is not an XmlDocument object");
            current.AppendChild(owner.CreateXmlDeclaration("1.0", null, standalone ? "yes" : "no"));
            state = WriteState.Prolog;
        }

        /// <summary>
        /// Creates a System.Xml.XmlElement node.
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            /* applied fixes from www.gotdotnet.com
            if (state == WriteState.Attribute || state == WriteState.Element || state == WriteState.Start || state == WriteState.Prolog)
                state = WriteState.Content; 
            if (state != WriteState.Content)
                throw new InvalidOperationException("Writer is in the wrong state for writing element content");
    
            XmlElement e = owner.CreateElement(prefix, localName, ns);
            current.AppendChild(e);
            current = e; // push this element on the stack so to speak.
            state = WriteState.Element;
            */
            if (state == WriteState.Attribute || state == WriteState.Element || state == WriteState.Start || state == WriteState.Prolog)
                state = WriteState.Content;
            if (state != WriteState.Content)
                throw new InvalidOperationException("Writer is in the wrong state for writing element content");

            if (prefix == null)
            {
                prefix = current.GetPrefixOfNamespace(ns);
            }
            XmlElement e = owner.CreateElement(prefix, localName, ns);
            current.AppendChild(e);
            current = e; // push this element on the stack so to speak. 
            state = WriteState.Element;
        }
        /// <summary>
        /// Creates a System.Xml.XmlText node.  If the current node is already an XmlText
        /// node it appends the text to that node.
        /// </summary>
        public override void WriteString(string text)
        {
            XmlNode parent = current;
            if (state == WriteState.Attribute)
            {
                parent = ca;
            }
            else if (state == WriteState.Element)
            {
                state = WriteState.Content;
            }
            if (state != WriteState.Attribute && state != WriteState.Content)
                throw new InvalidOperationException("Writer is in the wrong state to be writing text content");

            XmlNode last = parent.LastChild;
            if (last == null || !(last is XmlText))
            {
                last = owner.CreateTextNode(text);
                parent.AppendChild(last);
            }
            else
            {
                XmlText t = last as XmlText;
                t.AppendData(text);
            }

        }
        /// <summary>
        /// Calls WriteString with the character data.
        /// </summary>
        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            WriteString(new string(new char[] { lowChar, highChar }));
        }
        /// <summary>
        /// Create a System.Xml.XmlWhitespace node.
        /// </summary>
        public override void WriteWhitespace(string ws)
        {
            if (state == WriteState.Attribute || state == WriteState.Element)
                state = WriteState.Content;
            if (state != WriteState.Content && state != WriteState.Prolog && state != WriteState.Start)
                throw new InvalidOperationException("Writer is not in the right state to be writing whitespace nodes");
            current.AppendChild(owner.CreateWhitespace(ws));
            if (state == WriteState.Start) state = WriteState.Prolog;
        }
    }
}