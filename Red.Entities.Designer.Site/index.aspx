<%@ Page Language="C#" Inherits="Red.Entities.Designer.Site.index" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>index</title>
</head>
<body>
	<form id="form1" runat="server">
		<asp:Repeater runat="server" id="repeater">
			<ItemTemplate>
				<div>
						<div><label>Code:</label><span><%# DataBinder.Eval(Container.DataItem, "Code") %></span></div>
				</div>
			</ItemTemplate>
		</asp:Repeater>
	</form>
</body>
</html>
