<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:b="http://library.by/catalog"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
                exclude-result-prefixes="msxsl">
  
    <xsl:output method="xml" indent="yes"/>

  <xsl:key name="key_id" match="b:book" use="."/>

  <xsl:template match="b:catalog">
    <html>
      <body>
        <xsl:apply-templates select="b:book"/>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="b:book">
    <xsl:if test="generate-id(./b:genre)=generate-id(key('key_id',/b:genre))">
      <xsl:value-of select="b:genre"/>
      <br/>
    </xsl:if>
  </xsl:template>

  
</xsl:stylesheet>
