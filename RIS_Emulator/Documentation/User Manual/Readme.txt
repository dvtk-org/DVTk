+++++++++++++++++++++++++++
+ Dvtk based RIS emulator +
+++++++++++++++++++++++++++



Requirements:
=============
- .Net 2.0 needs to be installed.



Features:
=========
- Modality Worklist C-Find is handled as a SCP.

- Modality Performed Procedure Step N-Create and N-Set are handled as a SCP.

- If the port number for Worklist and MPPS are the same, both kind of requests will 
  be handled on the same port and the configured AE titles for Worklist will be used.

- Possibility to generate random digits in attribute values (e.g. for creating unique ID's) each
  time the RIS emulator is started.



Usage:
======
- Unzip the zip file to any location, keeping the folder strcuture in the zip file intact.

- The data\worklist directory contains example Storage DCM files. They determine the
  responses sent back for Worklist requests. If needed. replace these files with other
  Storage DCM files and/or Modality worklist C-FIND-RSP DCM files.

- In the Worklist tab, use the "Edit Dicom Files..." to change the Dicom files in the
  data\worklist directory. The Dvtk based DCM Editor will be started to perform this
  task. The RIS Emulator will not be visible while using the DCM Editor. When the DCM
  Editor is closed, the RIS Emulator will automatically become visible again.

- The "View information model..." button in the Worklist tab may be used to inspect what
  Worklist information model will be constructed from the DCM files in the data\worklist 
  directory.

- To let the RIS emulator generate random digits in attribute values, insert the
  character '@' in attribute values (use the "Edit Dicom Files..." button for this). When using the 
  "View information model..." button, these '@' will still be visible. When the RIS emulator
  is started, each '@' will be replaced by a random digit (0-9). Multiple '@' characters may be inserted 
  next to each other in attribute values to create a large random number. When the RIS emulator
  is stopped and started again, these random digits will be generated again.
  It is only possible to insert the '@' character in attributes with the following VR's:
  . AE (Application Entity)
  . AS (Age String)
  . CS (Code String)
  . DA (Date)
  . DS (Decimal String)
  . DT (Date Time)
  . IS (Integer String)
  . LO (Long String)
  . PN (Person Name)
  . SH (Short String)
  . TM (Time)
  . UI (Unique Identifier)

- Start the emulator by clicking the start button.
