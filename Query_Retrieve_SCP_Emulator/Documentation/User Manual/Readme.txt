++++++++++++++++++++++++++++++++++++++
+ Dvtk based Query/Retrieve emulator +
++++++++++++++++++++++++++++++++++++++
	
Requirements:
=============
- .Net 2.0 needs to be installed.


Features:
=========
- Query/retrieve C-Find and C-Move requests are handled as a SCP.
  Supported Information Models are:
  . Patient Root Query/Retrieve Information Model.
  . Study Root Query/Retrieve Information Model.
  . Patient/Study Only Query/Retrieve Information Model.

- Possibility to generate random digits in attribute values (e.g. for creating unique ID's) each
  time the Query/Retrieve emulator is started.


Usage:
======
- The data\QueryRetrieve directory contains example Storage DCM files. They determine the
  responses sent back. Replace these file with other Storage DCM files if needed.

- In the Query/Retrieve tab, use the "Edit Dicom Files..." to change the Dicom files in the
  data\QueryRetrieve directory. 

- The "View information models..." button in the Query/Retrieve tab may be used to inspect what
  Query/Retrieve information models will be constructed from the DCM files in the data\worklist 
  directory.

- To let the Query/Retrieve emulator generate random digits in attribute values, insert the
  character '@' in attribute values (use the "Edit Dicom Files..." button for this). When using the 
  "View information model..." button, these '@' will still be visible. When the Query/Retrieve emulator
  is started, each '@' will be replaced by a random digit (0-9). Multiple '@' characters may be inserted 
  next to each other in attribute values to create a large random number. When the Query/Retrieve emulator
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
