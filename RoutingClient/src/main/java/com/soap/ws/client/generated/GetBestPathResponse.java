
package com.soap.ws.client.generated;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour anonymous complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="GetBestPathResult" type="{http://schemas.datacontract.org/2004/07/RoutingServer}MultipleCheckpointsItinary" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "getBestPathResult"
})
@XmlRootElement(name = "GetBestPathResponse", namespace = "http://tempuri.org/")
public class GetBestPathResponse {

    @XmlElementRef(name = "GetBestPathResult", namespace = "http://tempuri.org/", type = JAXBElement.class, required = false)
    protected JAXBElement<MultipleCheckpointsItinary> getBestPathResult;

    /**
     * Obtient la valeur de la propriété getBestPathResult.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link MultipleCheckpointsItinary }{@code >}
     *     
     */
    public JAXBElement<MultipleCheckpointsItinary> getGetBestPathResult() {
        return getBestPathResult;
    }

    /**
     * Définit la valeur de la propriété getBestPathResult.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link MultipleCheckpointsItinary }{@code >}
     *     
     */
    public void setGetBestPathResult(JAXBElement<MultipleCheckpointsItinary> value) {
        this.getBestPathResult = value;
    }

}
