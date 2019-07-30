namespace MifuminSoft.funyan.Core
{
class Cf3MIDIOutput : public CMIDIOutputDM
{
protected:
	bool m_bSecondary;
DWORD m_Option;
public:
	void SetPlayOption(DWORD option);
LRESULT Open(string pFileName, bool secondary);
LRESULT SetLoopPos(DWORD start, DWORD end);
LRESULT Play(void);
LRESULT SetSecondary();
Cf3MIDIOutput();
virtual ~Cf3MIDIOutput();

};
}