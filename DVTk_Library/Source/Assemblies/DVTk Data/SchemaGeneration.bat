: Part of DvtkData.dll - .NET class library providing basic data-classes for DVTK
: Copyright © 2001-2005
: Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

cd bin/Debug

echo After creating be sure to add all required xsd imports to the 'results' xsd.

rem Specify the types to generate the xsd for.

xsd /t:Details /t:Display /t:Import /t:MediaRead /t:MediaWrite /t:ReadMedia /t:Receive /t:Results /t:Send /t:DicomFile /t:A_ABORT /t:A_ASSOCIATE_AC /t:A_ASSOCIATE_RJ /t:A_ASSOCIATE_RQ /t:A_RELEASE_RP /t:A_RELEASE_RQ /t:DulMessage /t:DicomMessage /t:ActivityReport /t:ValidationObjectResult /t:ValidationAbortRq /t:ValidationAssociateAc /t:ValidationAssociateRj /t:ValidationAssociateRq /t:ValidationReleaseRp /t:ValidationReleaseRq DvtkData.dll