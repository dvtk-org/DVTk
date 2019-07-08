<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dimse="urn:schemas-dvtk:dimse" xmlns:activity="urn:schemas-dvtk:activity" xmlns:validation="urn:schemas-dvtk:validation" xmlns:media="urn:schemas-dvtk:media" xmlns:dul="urn:schemas-dvtk:dul" xmlns:results="urn:schemas-dvtk:results" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:variable name="skipped">
		<xsl:value-of select="document(//collection/FileName)/Nodes/skipped"/>
	</xsl:variable>
	<xsl:param name="resultdes"/>

	<xsl:template match="collection">
		<html>
			<head>
				<style>
					<xsl:text disable-output-escaping="yes">
&lt;!--
#foldheader{cursor:pointer;cursor:hand ; font-weight:bold ;
list-style-image:url(fold.gif)}
#foldinglist{list-style-image:url(list.gif)}
//--&gt;
			</xsl:text>
				</style>
				<script src="script.js"/>
			</head>
			<body>
				<font size="-2" face="helvetica">
					<script src="script.js"/>
					<ul>
						<xsl:for-each select="Error">
							<li id="foldheader">Error</li>
							<ul id="foldinglist" style="display:none">
								<xsl:for-each select="message">
									<xsl:call-template name="display">
								</xsl:call-template>
								</xsl:for-each>
							</ul>
						</xsl:for-each>
						<xsl:for-each select="Warning">
							<li id="foldheader">Warning</li>
							<ul id="foldinglist" style="display:none">
								<xsl:for-each select="message">
									<xsl:call-template name="display">
												</xsl:call-template>
								</xsl:for-each>
							</ul>
						</xsl:for-each>
						<xsl:for-each select="Info">
							<li id="foldheader">Info</li>
							<ul id="foldinglist" style="display:none">
								<xsl:for-each select="message">
									<xsl:call-template name="display">
								</xsl:call-template>
								</xsl:for-each>
							</ul>
						</xsl:for-each>
					</ul>
				</font>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="display">
	<xsl:param name="resultdes"/>
			<xsl:if test="not(contains($skipped,@Name))">
			<li id="foldheader">
				<xsl:element name="a">
									<xsl:attribute name="href"><xsl:value-of select="results/id"/> * <xsl:value-of select="results/Resultfile"/>  FOLDERLINK</xsl:attribute>
									<xsl:value-of select="@Name"/>
								</xsl:element>
			</li>
					<ul id="foldinglist" style="display:none"><xsl:for-each select="results">
		
				<xsl:if test="not(contains($skipped,concat(Resultfile,' ',@Name)))">
					<li>
					<xsl:element name="a">
									<xsl:attribute name="href"><xsl:value-of select="id"/> * <xsl:value-of select="Resultfile"/></xsl:attribute>
									<xsl:value-of select="Resultfile"/>
								</xsl:element>
					</li>
						</xsl:if>
							
						</xsl:for-each></ul></xsl:if>

	</xsl:template>
</xsl:stylesheet>
