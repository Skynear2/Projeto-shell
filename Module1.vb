
Imports System.IO



Module Module1
	Function print(aux As List(Of String))
		For Each iten In aux
			Console.WriteLine(iten)
		Next
		print = True
	End Function

	Function printFileInfo(listFil As List(Of String), listFilInfo As List(Of String))
		Dim i As Integer = 0

		For Each iten In listFil
			Console.Write(iten)
			Console.Write(" " + listFilInfo(i) + " " + listFilInfo(i + 1) + " " + listFilInfo(i + 2))
			Console.WriteLine("")
			i = i + 3
		Next
		printFileInfo = True
	End Function

	Function GetFileOwner(file As String)
		Dim fi As New FileInfo(file)
		Dim fs As System.Security.AccessControl.FileSecurity = fi.GetAccessControl
		Dim owner As System.Security.Principal.NTAccount = CType(fs.GetOwner(GetType(System.Security.Principal.NTAccount)), System.Security.Principal.NTAccount)
		Return owner.ToString
	End Function

	Function ls(currentLocal As String, command As String)
		Dim split As String() = command.Split(New Char() {" "c})
		Dim local As String() = currentLocal.Split(New Char() {"\"c})
		Dim files() As String = IO.Directory.GetFiles(currentLocal)
		Console.WriteLine("entrei no LS")
		Dim list As New List(Of String)
		list.Add("-dirs")
		list.Add("-files")
		list.Add("-full")
		list.Add("-sortasc")
		list.Add("-sortdesc")

		'confere se o split tem tamanho maior q 1
		If (split.Length > 1) Then
			'se split > 1 compara se  o primeiro argumento está na lista
			If ((String.Compare(split(1), list(0)) = 0)) Then

				Dim listDir As New List(Of String) 'Cria uma lista de diretorios

				For Each Dir As String In Directory.GetDirectories(currentLocal)
					Dim dirInfo As New System.IO.DirectoryInfo(Dir)
					listDir.Add(dirInfo.Name) 'add todos os diretorios na lista
				Next
				'se o split tem tamanho 3 compara o argumento entre sortasc e sortdesc
				If split.Length = 3 Then

					If String.Compare(split(2), list(3)) = 0 Then

						listDir.Sort() 'ordena a lista em ordem alfabetica
						print(listDir) 'chama função para printar lista de diretorios no console

					ElseIf String.Compare(split(2), list(4)) = 0 Then

						listDir.Reverse() 'ordena lista de diretorios de forma descendente
						print(listDir)

					End If
				Else
					print(listDir) 'caso não tenha argumento adicional apenas imprime diretorios (Default = sortasc)
				End If

			ElseIf String.Compare(split(1), list(1)) = 0 Then 'compara comando com segundo comando da lista de argumentos (files)

				Dim listFil As New List(Of String)
				Dim ListFilInfo As New List(Of String)
				Console.WriteLine("Files: ")

				For Each Fil As String In Directory.GetFiles(currentLocal)
					listFil.Add(System.IO.Path.GetFileName(Fil)) 'add todos os files na lista
					ListFilInfo.Add("Creation: " + File.GetCreationTime(Fil).ToString)
					ListFilInfo.Add("Size: " + Fil.Length.ToString)
					ListFilInfo.Add("Owner: " + GetFileOwner(Fil))
				Next
				If split.Length = 3 Then

					If String.Compare(split(2), list(3)) = 0 Then

						listFil.Sort() 'ordena a lista em ordem alfabetica
						print(listFil) 'chama função para printar lista de files no console

					ElseIf String.Compare(split(2), list(4)) = 0 Then

						listFil.Reverse() 'ordena lista  de forma descendente
						print(listFil)

					ElseIf String.Compare(split(2), list(2)) = 0 Then
						printFileInfo(listFil, ListFilInfo) 'imprime os arquivos com todas as infos no console
					End If

				Else
					print(listFil) 'caso não tenha argumento adicional apenas imprime diretorios (Default = sortasc)
				End If

			End If
		Else
			Dim listAll As New List(Of String)
			For Each Dir As String In Directory.GetDirectories(currentLocal)
				Dim dirInfo As New System.IO.DirectoryInfo(Dir)
				listAll.Add(dirInfo.Name) 'add todos os diretorios na lista
			Next
			For Each Fil As String In Directory.GetFiles(currentLocal)
				listAll.Add(System.IO.Path.GetFileName(Fil)) 'add todos os files na lista
			Next
			print(listAll)
		End If


		ls = True
	End Function

	Function mkdir(currentLocal As String, command As String)
		Dim name() As String = command.Split(" "c) 'Separa mkdir do argumento com nome do diretorio
		If name.Length > 2 Then
			Dim iterator As Integer = 2
			For i As Integer = 2 To name.Length - 1
				name(1) = name(1) + " " + name(i)
			Next
		End If
		Dim answer As String = ""
		If (Not System.IO.Directory.Exists(currentLocal + "\" + name(1))) Then 'se o diretorio n existe cria 
			System.IO.Directory.CreateDirectory(currentLocal + "\" + name(1)) 'cria diretorio
			Console.WriteLine("Diretorio criado com sucesso.")
		Else
			Console.WriteLine("Já existe um diretório com esse nome, deseja substituir? (S/N)") 'se ja existe pergunta se quer substituir
			answer = Console.ReadLine().ToString
			If String.Compare(answer, "s") = 0 Then
				Console.WriteLine("")
				System.IO.Directory.Delete(currentLocal + "\" + name(1), True) 'Deleta o diretorio antigo
				System.IO.Directory.CreateDirectory(currentLocal + "\" + name(1)) 'Substitui o diretorio criando outro
				Console.WriteLine("Diretorio criado com sucesso.")
			Else
				mkdir = True
			End If
		End If
		mkdir = True 'retorno
	End Function

	Function mkfile(ByVal currentLocal As String, command As String)
		Dim name() As String = command.Split(" "c) 'Separa mkfile do argumento com nome do diretorio
		Dim answer As String = ""
		Dim fs As FileStream = Nothing 'cria o fileSteram

		If (Not File.Exists(name(1))) Then
			fs = File.Create(currentLocal + "\" + name(1)) 'cria o arquivo
			Using fs
			End Using
			Console.WriteLine("Arquivo criado com sucesso.")
		Else
			Console.WriteLine("Já existe um arquivo com esse nome, deseja substituir? (s/n)") 'se ja existe pergunta se quer substituir
			answer = Console.ReadLine().ToString
			If String.Compare(answer, "s") = 0 Then
				Console.WriteLine("")
				System.IO.File.Delete(currentLocal + "\" + name(1))
				Dim fs2 As FileStream = File.Create(currentLocal + "\" + name(1))
				Console.WriteLine(currentLocal + "\" + name(1))
				Console.WriteLine("Arquivo criado com sucesso.")
			Else
				mkfile = True 'retorno
			End If
		End If
		mkfile = True 'retorno
	End Function

	Function rmdir(ByVal currentLocal As String, command As String)
		Dim name() As String = command.Split(" "c) 'Separa mkfile do argumento com nome do diretorio
		Dim answer As String = ""
		Console.WriteLine("Deseja mesmo deletar o diretorio " + name(1) + "? (s/n)")
		answer = Console.ReadLine().ToString
		If String.Compare(answer, "s") = 0 Then
			System.IO.Directory.Delete(currentLocal + "\" + name(1), True) 'Deleta o diretorio
			Console.WriteLine("Diretorio apagado com sucesso.")
		End If
		rmdir = True 'retorno
	End Function

	Function rmfile(ByVal currentLocal As String, command As String)
		Dim name() As String = command.Split(" "c) 'Separa mkfile do argumento com nome do diretorio
		Dim answer As String = ""
		Console.WriteLine("Deseja mesmo deletar o file " + name(1) + "? (s/n)")
		answer = Console.ReadLine().ToString
		If String.Compare(answer, "s") = 0 Then
			System.IO.File.Delete(currentLocal + "\" + name(1))
			Console.WriteLine("Arquivo apagado com sucesso.")
		End If
		rmfile = True 'retorno
	End Function

	Function copy(command As String)
		Dim comando() As String = command.Split(" "c) 'Separa comando da origem, destino e pasta a ser copiada
		Dim dir() As String = comando(2).Split("\"c)
		Dim DiretorioOrigem As String = ""
		For value As Integer = 0 To dir.Length - 2
			DiretorioOrigem = DiretorioOrigem + dir(value)
		Next
		If comando(1).Contains(".") Then
			If System.IO.File.Exists(comando(1)) = True Then
				System.IO.File.Copy(comando(1), comando(2))
				Console.WriteLine("Arquivo copiado com sucesso.")

			Else
				Console.WriteLine("O arquivo não existe.")
			End If
		Else
			Dim newDirectory As String = System.IO.Path.Combine(comando(2), Path.GetFileName(Path.GetDirectoryName(comando(1))))
			If Not (Directory.Exists(newDirectory)) Then
				Directory.CreateDirectory(newDirectory)
			End If
			Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(comando(1), newDirectory)
			'If Not (Directory.Exists(comando(2))) Then
			'Directory.CreateDirectory(comando(2))
			'Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(comando(1), comando(2))
			'Else
			'Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(comando(1), comando(2))
			'End If
		End If
		copy = True 'retorno
	End Function

	Function move(command As String)
		Dim comando() As String = command.Split(" "c) 'Separa comando da origem, destino e pasta a ser copiada
		Dim dir() As String = comando(2).Split("\"c)
		Dim DiretorioOrigem As String = ""
		For value As Integer = 0 To dir.Length - 2
			DiretorioOrigem = DiretorioOrigem + dir(value)
		Next
		If comando(1).Contains(".") Then
			If System.IO.File.Exists(comando(1)) = True Then
				System.IO.File.Move(comando(1), comando(2))
			Else
				Console.WriteLine("O arquivo não existe.")
			End If
		Else
			If Not (Directory.Exists(comando(2))) Then
				Directory.CreateDirectory(comando(2))
				Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(comando(1), comando(2))
			Else
				Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(comando(1), comando(2))
			End If
		End If
		move = True 'retorno
	End Function

	Function cat(currentLocal As String, command As String)
		Dim comando() As String = command.Split(" "c) 'Separa comando da origem, destino e pasta a ser copiada.
		Dim fileReader As String
		fileReader = My.Computer.FileSystem.ReadAllText(comando(1))
		Console.WriteLine(fileReader)
		cat = True
	End Function

	Function man(command As String)
		Dim comando() As String = command.Split(New Char() {" "c})
		Dim list As New List(Of String)
		list.Add("ls")
		list.Add("mkdir")
		list.Add("mkfile")
		list.Add("rmdir")
		list.Add("rmfile")
		list.Add("copy")
		list.Add("cd")
		list.Add("cat")
		list.Add("clear")
		If comando.Length > 1 Then
			If (String.Compare(comando(1), list(0)) = 0) Then
				Console.WriteLine("ls  Lista o diretório atual.")
				Console.WriteLine("ls -dirs Lista apenas diretórios do diretório atual.")
				Console.WriteLine("ls -files Lista apenas arquivos do diretório atual.")
				Console.WriteLine("ls -dirs ou -files com -full Lista todas as informações de arquivos e/ou diretorios do diretório atual.")
				Console.WriteLine("ls -sortasc Lista o diretório atual em ordem alfabética.")
				Console.WriteLine("ls -sortdesc  Lista o diretório atual de forma reversa.")
			ElseIf (String.Compare(comando(1), list(1)) = 0) Then
				Console.WriteLine("mkdir nome  Cria um diretório.")
			ElseIf (String.Compare(comando(1), list(2)) = 0) Then
				Console.WriteLine("mkfile nome.extensão  Cria um arquivo.")
			ElseIf (String.Compare(comando(1), list(3)) = 0) Then
				Console.WriteLine("rmdir nome  Remove um diretório.")
			ElseIf (String.Compare(comando(1), list(4)) = 0) Then
				Console.WriteLine("rmfile nome  Remove um arquivo.")
			ElseIf (String.Compare(comando(1), list(5)) = 0) Then
				Console.WriteLine("copy origem destino  Copia um dir ou file para outro local.")
			ElseIf (String.Compare(comando(1), list(6)) = 0) Then
				Console.WriteLine("cat arquivo  Imprime no console todo o conteudo de um arquivo.")
			ElseIf (String.Compare(comando(1), list(7)) = 0) Then
				Console.WriteLine("clear limpa o console.")
			End If
		Else
			Console.WriteLine("ls  Lista o diretório atual.")
			Console.WriteLine("ls -dirs Lista apenas diretórios do diretório atual.")
			Console.WriteLine("ls -files Lista apenas arquivos do diretório atual.")
			Console.WriteLine("ls -dirs ou -files com -full Lista todas as informações de arquivos e/ou diretorios do diretório atual.")
			Console.WriteLine("ls -sortasc Lista o diretório atual em ordem alfabética.")
			Console.WriteLine("ls -sortdesc  Lista o diretório atual de forma reversa.")
			Console.WriteLine("mkdir nome  Cria um diretório.")
			Console.WriteLine("mkfile nome.extensão  Cria um arquivo.")
			Console.WriteLine("rmdir nome  Remove um diretório.")
			Console.WriteLine("rmfile nome  Remove um arquivo.")
			Console.WriteLine("copy origem destino  Copia um dir ou file para outro local.")
			Console.WriteLine("cat arquivo  Imprime no console todo o conteudo de um arquivo.")
			Console.WriteLine("clear limpa o console.")
		End If
		man = True
	End Function


	Sub main()
		Dim command As String = ""
		Dim comando() As String
		Dim currentLocal As String = System.IO.Directory.GetCurrentDirectory
		Dim newLocal As String = ""
		Dim local() = currentLocal.Split("\"c)
		'Console.WriteLine(local.Length)

		Do While command <> "exit"
			Console.Write(currentLocal)
			Console.Write(">")
			command = Console.ReadLine()
			comando = command.Split(New Char() {" "c})


			If comando(0).Contains("ls") Then
				ls(currentLocal, command)
			ElseIf comando(0).Contains("mkdir") Then
				mkdir(currentLocal, command)
			ElseIf comando(0).Contains("mkfile") Then
				mkfile(currentLocal, command)
			ElseIf comando(0).Contains("rmdir") Then
				rmdir(currentLocal, command)
			ElseIf comando(0).Contains("rmfile") Then
				rmfile(currentLocal, command)
			ElseIf comando(0).Contains("copy") Then
				copy(command)
			ElseIf comando(0).Contains("move") Then
				move(command)
			ElseIf comando(0).Contains("cd") Then
				Dim comando2() As String = command.Split(" "c)
				If (String.Compare(comando2(1), "..") = 0) Then
					For aux As Integer = 0 To local.Length - 2
						newLocal = newLocal + "\" + local(aux)
					Next
					currentLocal = newLocal
				Else
					currentLocal = comando2(1)
				End If
			ElseIf comando(0).Contains("cat") Then
				cat(currentLocal, command)
			ElseIf comando(0).Contains("man") Then
				man(command)
			ElseIf comando(0).Contains("clear") Then
				Console.Clear()
			ElseIf comando(0).Contains("local") Then
				Console.WriteLine("Current local: " + currentLocal)
			End If

		Loop

	End Sub

End Module
