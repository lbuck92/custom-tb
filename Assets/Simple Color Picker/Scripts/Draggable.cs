using UnityEngine;

public class Draggable : MonoBehaviour
{
	public SteamVR_TrackedObject trackedObj;

	public bool fixX;
	public bool fixY;
	public Transform thumb;	
	bool dragging;

	void FixedUpdate()
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			dragging = false;
			Ray ray = new Ray (trackedObj.transform.position, trackedObj.transform.forward);
			//var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
				dragging = true;
			}
		}

		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) dragging = false;
		if (dragging && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			Ray ray = new Ray (trackedObj.transform.position, trackedObj.transform.forward);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
				var point = hit.point;
				point = GetComponent<Collider>().ClosestPointOnBounds(point);
				SetThumbPosition(point);
				SendMessage("OnDrag", Vector3.one - (thumb.position - GetComponent<Collider>().bounds.min) / GetComponent<Collider>().bounds.size.x);
			}
	

		}
	}

	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
		thumb.position = new Vector3(fixX ? thumb.position.x : point.x, fixY ? thumb.position.y : point.y, thumb.position.z);
	}
}
