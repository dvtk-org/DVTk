<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="create_results.xslt"/>
	<xsl:template match="Collection">

 <collection>  
		<xsl:for-each select="//result">
						 <Resultfile>  
						 <filename><xsl:value-of select="."/></filename>
						 <xsl:apply-templates select="document(.)"/>
							
						 </Resultfile>
		</xsl:for-each>
		<xsl:for-each select="Directory">
						 <Directory><xsl:value-of select="."/></Directory>
		</xsl:for-each>
				<xsl:for-each select="FileName">
						 <FileName><xsl:value-of select="."/></FileName>
		</xsl:for-each>
		</collection>
	</xsl:template>
</xsl:stylesheet>
