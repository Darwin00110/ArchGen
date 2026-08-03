import os
import winreg

pastaExecutavel_ArchGen = r"C:\Program Files\ArchGen"
caminhoExe = os.path.join(pastaExecutavel_ArchGen, "ArchGen.exe")

if not os.path.exists(caminhoExe):
    print("O executavel do 'ArchGen' não existe, encerrando o processo.")
    os._exit(0)

# Lê o PATH atual apenas do usuário (HKCU), não o combinado com o sistema
with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_READ) as chave:
    try:
        path_usuario, _ = winreg.QueryValueEx(chave, "Path")
    except FileNotFoundError:
        path_usuario = ""

if pastaExecutavel_ArchGen.lower() not in path_usuario.lower():
    novo_path = f"{path_usuario};{pastaExecutavel_ArchGen}" if path_usuario else pastaExecutavel_ArchGen
    with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_SET_VALUE) as chave:
        winreg.SetValueEx(chave, "Path", 0, winreg.REG_EXPAND_SZ, novo_path)
    print("PATH atualizado! Abra um novo terminal para o comando 'ArchGen' funcionar.")
else:
    print("PATH já contém a pasta do ArchGen.")