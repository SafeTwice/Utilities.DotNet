# Utilities.DotNet.Cyptography

## About

The _Utilities.DotNet.Cyptography_ package provides cryptography-related classes.

## Usage

### Elliptic Curve Digital Signature Algorithm (ECDSA-SHA) support

As of .NET 9, the ECDSA-SHA algorithm is not supported out-of-the-box for signing.

In order to sign using the ECDSA-SHA algorithm, you can use the signature description classes provided by this package:


| Class                             | Description                               |
|-----------------------------------|-------------------------------------------|
| `ECDsaSha1SignatureDescription`   | ECDSA-SHA1 signature algorithm            |
| `ECDsaSha256SignatureDescription` | ECDSA-SHA256 signature algorithm          |
| `ECDsaSha384SignatureDescription` | ECDSA-SHA384 signature algorithm          |
| `ECDsaSha512SignatureDescription` | ECDSA-SHA512 signature algorithm          |

#### Signature Algorithm Registration

Before performing a signature creation of verification operation using a ECDSA-SHA*
algorithm, call the static `Register()` method in the corresponding `ECDsaSha*SignatureDescription` class, or just call `ECDsaShaSignatureDescription.RegisterAll()` to register all the available ECDSA-SHA* algorithms.

You may safely call the registration methods multiple times or call them from a static constructor.

#### Example

``` CS
private void Sign( XmlDocument xmlDoc )
{
    // Register ECDSA-SHA algorithm.
    ECDsaSha256SignatureDescription.Register();

    // Retrieve key from store.
    X509Store store = new X509Store( StoreLocation.CurrentUser );
    store.Open( OpenFlags.ReadOnly );
    X509Certificate2Collection certs = store.Certificates.Find( X509FindType.FindBySubjectName, "CN=Whatever", false );
    if( certs.Count == 0 )
    {
        return;
    };
    store.Close();
    var cert = certs[ 0 ];

    // Prepare XML signing
    var signedXml = new SignedXml( xmlDoc )
    {
        SigningKey = cert.GetECDsaPrivateKey()!
    };
    signedXml.SignedInfo!.SignatureMethod = ECDsaSha256SignatureDescription.Name;
    signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";

    // Sign the whole document.
    var reference = new Reference( string.Empty );
    reference.AddTransform( new XmlDsigEnvelopedSignatureTransform() );
    reference.AddTransform( new XmlDsigExcC14NTransform() );
    signedXml.AddReference( reference );

    // Compute the signature
    signedXml.ComputeSignature();

    // Add the signature
    xml.DocumentElement.AppendChild( signedXml.GetXml() );
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_cyptography)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_cyptography)
