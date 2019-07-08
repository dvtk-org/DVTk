using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DvtkHighLevelInterface.Dicom.Other
{
    /// <summary>
    /// Structure of DICOM peers. It holds AR,Name,IP and port
    /// </summary>
    [Serializable]
    public struct DICOMPeer
    {
        public string Name;
        public string AE;
        public string IP;
        public ushort Port;
        /// <summary>
        /// wirtes the peer list in to file in the given path
        /// </summary>
        /// <param name="path">Config file location </param>
        /// <param name="peers">List of Peers to wirte</param>
        public static void WriteIntoFile(string path,List<DICOMPeer> peers)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            System.Xml.Serialization.XmlSerializer serialize = new System.Xml.Serialization.XmlSerializer(typeof(List<DICOMPeer>));
            serialize.Serialize(fs, peers);
            fs.Close();
            fs.Dispose();
            serialize = null;
        }
        /// <summary>
        /// Reads the peers from the config file location
        /// </summary>
        /// <param name="info">Config file location</param>
        /// <returns>returns the list of peers read from the file</returns>
        public static List<DICOMPeer> ReadFromFile(FileInfo info)
        {
            List<DICOMPeer> peers = new List<DICOMPeer>();
            if (info.Exists)
            {
                FileStream fs = new FileStream(info.FullName,FileMode.Open, FileAccess.Read);
                System.Xml.Serialization.XmlSerializer serialize = new System.Xml.Serialization.XmlSerializer(typeof(List<DICOMPeer>));
                peers=(List<DICOMPeer>) serialize.Deserialize(fs);
                fs.Close();
                fs.Dispose();
                serialize = null;
            }
            return peers;
        }
    }
}
