using UnityEngine;

public class StatusConditionEffect : MonoBehaviour, IEffect
{
    protected StatusCondition[] conditions;

    [Range(0f, 1f)]
    public float successChance = 1f;

    void Awake()
    {
        this.conditions = this.GetComponentsInChildren<StatusCondition>();

        foreach (var cond in this.conditions)
        {
            if (cond.gameObject == this.gameObject)
            {
                Debug.LogError("Les condicions d'estat han d'estar en un GameObject diferent del de la habilitat o l'efecte");
            }
        }
    }

    public void Resolve(Creature emitter, Creature receiver)
    {
        if (this.HasSucceeded() == false)
        {
            return;
        }

        foreach (var cond in this.conditions)
        {
            // Clonem l'objecte amb la condició d'estat
            GameObject parasiteObj = Instantiate(cond.gameObject);
            parasiteObj.SetActive(true);

            StatusCondition condition = parasiteObj.GetComponent<StatusCondition>();
            condition.Configure(receiver);

            receiver.AddStatusCondition(condition);
        }
    }

    private bool HasSucceeded()
    {
        if (this.successChance == 1f)
        {
            return true;
        }

        float dice = Random.Range(0f, 1f);

        return dice < this.successChance;
    }
}