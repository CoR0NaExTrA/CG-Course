using NAudio.Wave;
using Silk.NET.OpenAL;

namespace Battleship3D.Services;

public unsafe class AudioService
{
    private AL al;
    private ALContext alc;

    private Device* device;
    private Context* context;

    public AudioService()
    {
        alc = ALContext.GetApi();
        al = AL.GetApi();

        device = alc.OpenDevice( "" );
        context = alc.CreateContext( device, null );

        alc.MakeContextCurrent( context );
    }

    public unsafe uint LoadWav( string path )
    {
        using WaveFileReader wav =
            new( path );

        byte[] bufferData =
            new byte[ wav.Length ];

        wav.Read( bufferData, 0, bufferData.Length );

        uint buffer =
            al.GenBuffer();

        BufferFormat format =
            wav.WaveFormat.Channels == 1
            ? BufferFormat.Mono16
            : BufferFormat.Stereo16;

        fixed ( byte* d = bufferData )
        {
            al.BufferData(
                buffer,
                format,
                d,
                bufferData.Length,
                wav.WaveFormat.SampleRate );
        }

        Console.WriteLine( al.GetError() );

        return buffer;
    }

    public uint CreateSource( uint buffer, bool loop = false )
    {
        uint source = al.GenSource();

        al.SetSourceProperty( source, SourceInteger.Buffer, ( int )buffer );

        if ( loop )
        {
            al.SetSourceProperty( source, SourceBoolean.Looping, true );
        }

        return source;
    }

    public void Play( uint source )
    {
        al.SourcePlay( source );
        var err = al.GetError();

        Console.WriteLine( err );
    }
}