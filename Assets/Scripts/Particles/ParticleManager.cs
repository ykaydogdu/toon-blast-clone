using UnityEngine;

class ParticleManager : Singleton<ParticleManager>
{
    public ParticleSystem RedCubeParticle;
    public ParticleSystem GreenCubeParticle;
    public ParticleSystem BlueCubeParticle;
    public ParticleSystem YellowCubeParticle;
    public ParticleSystem BoxParticle;
    public ParticleSystem StoneParticle;
    public ParticleSystem VaseParticle;

    public void PlayParticle(Item item)
    {
        if (item == null || item.transform == null) return;
        ParticleSystem sys;
        switch (item.ItemType)
        {
            case ItemType.RedCube:
                sys = RedCubeParticle;
                break;
            case ItemType.GreenCube:
                sys = GreenCubeParticle;
                break;
            case ItemType.BlueCube:
                sys = BlueCubeParticle;
                break;
            case ItemType.YellowCube:
                sys = YellowCubeParticle;
                break;
            case ItemType.Box:
                sys = BoxParticle;
                break;
            case ItemType.Stone:
                sys = StoneParticle;
                break;
            case ItemType.Vase:
                sys = VaseParticle;
                break;
            default:
                return;
        }
        Vector3 pos = new Vector3(item.transform.position.x, item.transform.position.y, -10);
        var particle = Instantiate(sys, pos, Quaternion.identity, item.Cell.Board.particlesParent);
        particle.Play();
    }
}