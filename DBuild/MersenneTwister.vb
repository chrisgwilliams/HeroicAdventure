'
' An implementation of the Mersenne Twister algorithm (MT19937), developed
' with reference to the C code written by Takuji Nishimura and Makoto Matsumoto
' (http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html).
'
' This code is free to use for any pupose.
'

Option Strict On

''' 
''' A random number generator with a uniform distribution using the Mersenne 
''' Twister algorithm.
''' 
Public Class MersenneTwister
    Private Const N As Integer = 624
    Private Const M As Integer = 397
    Private Const MATRIX_A As UInteger = &H9908B0DFUI
    Private Const UPPER_MASK As UInteger = &H80000000UI
    Private Const LOWER_MASK As UInteger = &H7FFFFFFFUI

    Private mt(N - 1) As UInteger
    Private mti As Integer = N + 1

    ''' <summary>
    ''' Create a new Mersenne Twister random number generator.
    ''' </summary>
    Public Sub New()
        Me.New(CUInt(Date.Now.Millisecond))
    End Sub

    ''' <summary>
    ''' Create a new Mersenne Twister random number generator with a
    ''' particular seed.
    ''' </summary>
    ''' <param name="seed">The seed for the generator.</param>
    <CLSCompliant(False)> _
    Public Sub New(ByVal seed As UInteger)
        mt(0) = seed
        For mti = 1 To N - 1
            mt(mti) = CUInt((1812433253UL * (mt(mti - 1) Xor (mt(mti - 1) >> 30)) + CUInt(mti)) And &HFFFFFFFFUL)
        Next
    End Sub

    ''' <summary>
    ''' Create a new Mersenne Twister random number generator with a
    ''' particular initial key.
    ''' </summary>
    ''' <param name="initialKey">The initial key.</param>
    <CLSCompliant(False)> _
    Public Sub New(ByVal initialKey() As UInteger)
        Me.New(19650218UI)

        Dim i, j, k As Integer
        i = 1 : j = 0
        k = CInt(IIf(N > initialKey.Length, N, initialKey.Length))

        For k = k To 1 Step -1
            mt(i) = CUInt(((mt(i) Xor ((mt(i - 1) Xor (mt(i - 1) >> 30)) * 1664525UL)) + initialKey(j) + CUInt(j)) And &HFFFFFFFFUI)
            i += 1 : j += 1
            If i >= N Then mt(0) = mt(N - 1) : i = 1
            If j >= initialKey.Length Then j = 0
        Next

        For k = N - 1 To 1 Step -1
            mt(i) = CUInt(((mt(i) Xor ((mt(i - 1) Xor (mt(i - 1) >> 30)) * 1566083941UL)) - CUInt(i)) And &HFFFFFFFFUI)
            i += 1
            If i >= N Then mt(0) = mt(N - 1) : i = 1
        Next

        mt(0) = &H80000000UI
    End Sub

    ''' <summary>
    ''' Generates a random number between 0 and System.UInt32.MaxValue.
    ''' </summary>
    <CLSCompliant(False)> _
    Public Function NextUInt32() As UInteger
        Dim y As UInteger
        Static mag01() As UInteger = {&H0UI, MATRIX_A}

        If mti >= N Then
            Dim kk As Integer

            Debug.Assert(mti <> N + 1, "Failed initialization")

            For kk = 0 To N - M - 1
                y = (mt(kk) And UPPER_MASK) Or (mt(kk + 1) And LOWER_MASK)
                mt(kk) = mt(kk + M) Xor (y >> 1) Xor mag01(CInt(y And &H1))
            Next

            For kk = kk To N - 2
                y = (mt(kk) And UPPER_MASK) Or (mt(kk + 1) And LOWER_MASK)
                mt(kk) = mt(kk + (M - N)) Xor (y >> 1) Xor mag01(CInt(y And &H1))
            Next

            y = (mt(N - 1) And UPPER_MASK) Or (mt(0) And LOWER_MASK)
            mt(N - 1) = mt(M - 1) Xor (y >> 1) Xor mag01(CInt(y And &H1))

            mti = 0
        End If

        y = mt(mti)
        mti += 1

        ' Tempering
        y = y Xor (y >> 11)
        y = y Xor ((y << 7) And &H9D2C5680UI)
        y = y Xor ((y << 15) And &HEFC60000UI)
        y = y Xor (y >> 18)

        Return y
    End Function

    ''' <summary>
    ''' Generates a random integer between 0 and System.Int32.MaxValue.
    ''' </summary>
    Public Function [Next]() As Integer
        Return CInt(NextUInt32() >> 1)
    End Function

    ''' <summary>
    ''' Generates a random integer between 0 and maxValue.
    ''' </summary>
    ''' <param name="maxValue">The maximum value. Must be greater than zero.</param>
    Public Function [Next](ByVal maxValue As Integer) As Integer
        Return [Next](0, maxValue)
    End Function

    ''' <summary>
    ''' Generates a random integer between minValue and maxValue.
    ''' </summary>
    ''' <param name="maxValue">The lower bound.</param>
    ''' <param name="minValue">The upper bound.</param>
    Public Function [Next](ByVal minValue As Integer, ByVal maxValue As Integer) As Integer
        Return CInt(Math.Floor((maxValue - minValue + 1) * NextDouble() + minValue))
    End Function

    ''' <summary>
    ''' Generates a random floating point number between 0 and 1.
    ''' </summary>
    Public Function NextDouble() As Double
        Return NextUInt32() * (1.0 / 4294967295.0)
    End Function
End Class