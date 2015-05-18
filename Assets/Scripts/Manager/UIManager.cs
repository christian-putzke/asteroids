using UnityEngine;

/**
 * The UI manager handles which panel is displayed
 */
public class UIManager : Singleton<UIManager>
{
	/**
	 * Includes all used panels
	 */
	public Panel[] panelArray;


	/**
	 * The current active panel
	 */
	private Enum.Panel activePanel = Enum.Panel.MainMenu;


	/**
	 * Shows the main menu on start up
	 */
	private void Start()
	{
		this.Show(Enum.Panel.MainMenu);
	}


	/**
	 * Shows the panel with the given id and hides the active one
	 */
	public void Show(Enum.Panel panel)
	{
		if (this.activePanel != panel)
		{
			this.GetPanel(this.activePanel).Disable();
		}

		this.GetPanel(panel).Enable();
		this.activePanel = panel;
	}



	/**
	 * Returns the panel by the given panel type
	 */
	public Panel GetPanel(Enum.Panel panel)
	{
		for (var index = 0; index < this.panelArray.Length; index ++)
		{
			if (this.panelArray[index].panel == panel)
			{
				return this.panelArray[index];
			}
		}

		throw new UnityException("Panel not found!");
	}


	/**
	 * Returns the panel by the given panel type
	 */
	public Type GetPanel<Type>(Enum.Panel panel) where Type : Panel
	{
		return (Type) this.GetPanel(panel);
	}
}
