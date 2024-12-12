/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Security.Cryptography;
using static Utilities.DotNet.Cryptography.I18N.LibraryLocalizer;

namespace Utilities.DotNet.Cryptography
{
    /// <summary>
    /// Contains the description of the ECDSA-SHA signature.
    /// </summary>
    public abstract class ECDsaShaSignatureDescription : SignatureDescription
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Registers all the ECDSA-SHA* signature algorithm.
        /// </summary>
        public static void RegisterAll()
        {
            ECDsaSha1SignatureDescription.Register();
            ECDsaSha256SignatureDescription.Register();
            ECDsaSha384SignatureDescription.Register();
            ECDsaSha512SignatureDescription.Register();
        }

        /// <inheritdoc/>
        public sealed override AsymmetricSignatureFormatter CreateFormatter( AsymmetricAlgorithm key )
        {
            var ecdsa = CheckKey( key );

            return new ECDsaSignatureFormatter( ecdsa, m_minKeySize, m_maxKeySize );
        }

        /// <inheritdoc/>
        public sealed override AsymmetricSignatureDeformatter CreateDeformatter( AsymmetricAlgorithm key )
        {
            var ecdsa = CheckKey( key );

            return new ECDsaSignatureDeformatter( ecdsa, m_minKeySize, m_maxKeySize );
        }

        /// <inheritdoc/>
        public sealed override HashAlgorithm CreateDigest() => m_digestCreator();

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected private ECDsaShaSignatureDescription( int minKeySize, int maxKeySize, Func<HashAlgorithm> digestCreator )
        {
            KeyAlgorithm = typeof( ECDsa ).AssemblyQualifiedName;

            m_minKeySize = minKeySize;
            m_maxKeySize = maxKeySize;
            m_digestCreator = digestCreator;
        }

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private sealed class ECDsaSignatureFormatter : AsymmetricSignatureFormatter
        {
            public ECDsaSignatureFormatter( ECDsa key, int minKeySize, int maxKeySize )
            {
                m_key = key;
                m_minKeySize = minKeySize;
                m_maxKeySize = maxKeySize;
            }

            public override void SetKey( AsymmetricAlgorithm key )
            {
                m_key = CheckKey( key, m_minKeySize, m_maxKeySize );
            }

            public override void SetHashAlgorithm( string strName )
            {
            }

            public override byte[] CreateSignature( byte[] rgbHash )
            {
                return m_key.SignHash( rgbHash );
            }

            private readonly int m_minKeySize;
            private readonly int m_maxKeySize;

            private ECDsa m_key;
        }

        private sealed class ECDsaSignatureDeformatter : AsymmetricSignatureDeformatter
        {
            public ECDsaSignatureDeformatter( ECDsa key, int minKeySize, int maxKeySize )
            {
                m_key = key;
                m_minKeySize = minKeySize;
                m_maxKeySize = maxKeySize;
            }

            public override void SetKey( AsymmetricAlgorithm key )
            {
                m_key = CheckKey( key, m_minKeySize, m_maxKeySize );
            }

            public override void SetHashAlgorithm( string strName )
            {
            }

            public override bool VerifySignature( byte[] rgbHash, byte[] rgbSignature )
            {
                return m_key.VerifyHash( rgbHash, rgbSignature );
            }

            private readonly int m_minKeySize;
            private readonly int m_maxKeySize;

            private ECDsa m_key;
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private ECDsa CheckKey( AsymmetricAlgorithm key ) => CheckKey( key, m_minKeySize, m_maxKeySize );

        private static ECDsa CheckKey( AsymmetricAlgorithm key, int minKeySize, int maxKeySize )
        {
            if( !( key is ECDsa ecdsa ) )
            {
                throw new ArgumentException( Localize( $"Key must be an ECDsa" ) );
            }
            else if( ecdsa.KeySize < minKeySize )
            {
                throw new ArgumentException( Localize( $"Key must have a size of at least {minKeySize} bits" ) );
            }
            else if( ecdsa.KeySize > maxKeySize )
            {
                throw new ArgumentException( Localize( $"Key must have a size of at most {maxKeySize} bits" ) );
            }

            return ecdsa;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly int m_minKeySize;
        private readonly int m_maxKeySize;
        private readonly Func<HashAlgorithm> m_digestCreator;
    }
}
